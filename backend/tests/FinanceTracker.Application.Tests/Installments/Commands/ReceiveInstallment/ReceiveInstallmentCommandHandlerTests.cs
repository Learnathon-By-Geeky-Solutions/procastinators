using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceTracker.Application.Installments.Commands.ReceiveInstallment;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Installments.Commands.ReceiveInstallments.Tests;

public class ReceiveInstallmentCommandHandlerTests
{
    private readonly Mock<ILogger<ReceiveInstallmentCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly ReceiveInstallmentCommandHandler _handler;
    private readonly string _borrowerId = "borrower-id";
    private readonly string _lenderId = "lender-id";
    private readonly int _loanId = 1;
    private readonly int _walletId = 1;
    private readonly string _userId = "user-id";
    private readonly UserDto _user;

    public ReceiveInstallmentCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ReceiveInstallmentCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();
        _userContextMock = new Mock<IUserContext>();

        _user = new UserDto(_userId, "test@test.com");
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new ReceiveInstallmentCommandHandler(
            _userContextMock.Object,
            _loanRepositoryMock.Object,
            _installmentRepositoryMock.Object,
            _walletRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReceiveInstallment()
    {
        // Arrange
        var currentDate = DateTime.Now;
        var nextDueDate = currentDate.AddMonths(1);
        decimal receivedAmount = 500m;
        decimal initialLenderBalance = 1000m;
        decimal initialLoanDueAmount = 2000m;

        var loan = new Loan
        {
            Id = _loanId,
            LenderId = _userId, // Important: The user is the lender in this scenario
            LoanRequestId = null, // This must be null per the check in the handler
            DueAmount = initialLoanDueAmount,
            DueDate = currentDate,
            IsDeleted = false,
        };

        var lenderWallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId, // This wallet belongs to the current user (lender)
            Balance = initialLenderBalance,
            IsDeleted = false,
        };

        var command = new ReceiveInstallmentCommand
        {
            LoanId = _loanId,
            Amount = receivedAmount,
            NextDueDate = nextDueDate,
            Note = "Test received",
            WalletId = _walletId,
        };

        // Setup mock repositories
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsLenderAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync(lenderWallet);

        _installmentRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Installment>()))
            .Callback<Installment>(installment =>
            {
                installment.Id = 1;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.Equal(1, result);

        // Verify lender wallet balance increased
        Xunit.Assert.Equal(initialLenderBalance + receivedAmount, lenderWallet.Balance);

        // Verify loan due amount decreased
        Xunit.Assert.Equal(initialLoanDueAmount - receivedAmount, loan.DueAmount);

        // Verify loan due date updated
        Xunit.Assert.Equal(nextDueDate, loan.DueDate);

        // Verify installment was created with correct values
        _installmentRepositoryMock.Verify(
            repo =>
                repo.CreateAsync(
                    It.Is<Installment>(i =>
                        i.LoanId == _loanId
                        && i.Amount == receivedAmount
                        && i.NextDueDate == nextDueDate
                        && i.Note == "Test received"
                    )
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_WithNonExistentLoan_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new ReceiveInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test received",
            WalletId = _walletId,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsLenderAsync(_loanId, _userId))
            .ReturnsAsync((Loan?)null);

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        Xunit.Assert.Equal($"Loan with id {_loanId} was not found", exception.Message);
    }

    [Fact]
    public async Task Handle_WithLoanRequestId_ShouldThrowForbiddenException()
    {
        // Arrange
        var loan = new Loan
        {
            Id = _loanId,
            LenderId = _userId,
            LoanRequestId = 1, // This will trigger the ForbiddenException in the handler
            IsDeleted = false,
        };

        var command = new ReceiveInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test received",
            WalletId = _walletId,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsLenderAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithNonExistentWallet_ShouldThrowNotFoundException()
    {
        // Arrange
        var loan = new Loan
        {
            Id = _loanId,
            LenderId = _userId,
            LoanRequestId = null,
            IsDeleted = false,
        };

        var command = new ReceiveInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test received",
            WalletId = _walletId,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsLenderAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync((Wallet?)null);

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithDeletedWallet_ShouldThrowNotFoundException()
    {
        // Arrange
        var loan = new Loan
        {
            Id = _loanId,
            LenderId = _userId,
            LoanRequestId = null,
            IsDeleted = false,
        };

        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId,
            IsDeleted = true, // Deleted wallet
        };

        var command = new ReceiveInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test received",
            WalletId = _walletId,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsLenderAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync(wallet);

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithWalletBelongingToDifferentUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var loan = new Loan
        {
            Id = _loanId,
            LenderId = _userId,
            LoanRequestId = null,
            IsDeleted = false,
        };

        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = "different-user-id", // Different user ID
            IsDeleted = false,
        };

        var command = new ReceiveInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test received",
            WalletId = _walletId,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsLenderAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync(wallet);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithAmountExceedingDueAmount_ShouldThrowBadRequestException()
    {
        // Arrange
        decimal dueAmount = 300m;
        decimal requestAmount = 500m; // Greater than dueAmount

        var loan = new Loan
        {
            Id = _loanId,
            LenderId = _userId,
            LoanRequestId = null,
            DueAmount = dueAmount,
            IsDeleted = false,
        };

        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId,
            IsDeleted = false,
        };

        var command = new ReceiveInstallmentCommand
        {
            LoanId = _loanId,
            Amount = requestAmount,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test received",
            WalletId = _walletId,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsLenderAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        _walletRepositoryMock.Setup(repo => repo.GetById(_walletId)).ReturnsAsync(wallet);

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
