using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Application.Installments.Commands.PayInstallment;
using FinanceTracker.Application.LoanClaims.Commands.ClaimLoanFund;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace FinanceTracker.Application.Installments.Commands.PayInstallments.Tests;

public class PayInstallmentCommandHandlerTests
{
    private readonly Mock<ILogger<PayInstallmentCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly PayInstallmentCommandHandler _handler;
    private readonly string _borrowerId = "borrower-id";
    private readonly string _lenderId = "lender-id";
    private readonly int _loanId = 1;
    private readonly string _userId = "user-id";
    private readonly UserDto _user;

    public PayInstallmentCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<PayInstallmentCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();
        _userContextMock = new Mock<IUserContext>();

        _user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new PayInstallmentCommandHandler(
            _userContextMock.Object,
            _loanRepositoryMock.Object,
            _installmentRepositoryMock.Object,
            _walletRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_PayInstallment()
    {
        // Arrange
        var currentDate = DateTime.Now;
        var nextDueDate = currentDate.AddMonths(1);
        decimal paymentAmount = 500m;
        decimal initialBorrowerBalance = 1000m;
        decimal initialLoanDueAmount = 2000m;

        var testUser = new UserDto(_userId, "test@example.com");

        var loanRequest = new LoanRequest { BorrowerId = _borrowerId, LenderId = _lenderId };

        var loan = new Loan
        {
            Id = _loanId,
            BorrowerId = _userId,
            LoanRequest = loanRequest,
            DueAmount = initialLoanDueAmount,
            DueDate = currentDate,
            IsDeleted = false,
        };

        var borrowerWallet = new Wallet
        {
            Id = 1,
            UserId = _userId,
            Balance = initialBorrowerBalance,
            IsDeleted = false,
        };

        var command = new PayInstallmentCommand
        {
            LoanId = _loanId,
            Amount = paymentAmount,
            NextDueDate = nextDueDate,
            Note = "Test payment",
            WalletId = borrowerWallet.Id,
        };

        var createdInstallment = new Installment
        {
            Id = 1,
            LoanId = _loanId,
            Amount = paymentAmount,
            NextDueDate = nextDueDate,
            Note = "Test payment",
        };

        // Setup mock repositories

        _userContextMock.Setup(context => context.GetUser()).Returns(testUser);

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsBorrowerAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(borrowerWallet.Id))
            .ReturnsAsync(borrowerWallet);

        _installmentRepositoryMock
            .Setup(repo => repo.CreateWithClaimAsync(It.IsAny<Installment>()))
            .Callback<Installment>(installment =>
            {
                installment.Id = 1;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.Equal(1, result);

        // Verify borrower wallet balance decreased
        Xunit.Assert.Equal(initialBorrowerBalance - paymentAmount, borrowerWallet.Balance);

        // Verify lender wallet balance increased

        // Verify loan due amount decreased
        Xunit.Assert.Equal(initialLoanDueAmount - paymentAmount, loan.DueAmount);

        // Verify loan due date updated
        Xunit.Assert.Equal(nextDueDate, loan.DueDate);

        // Verify installment was created with correct values
        _installmentRepositoryMock.Verify(
            repo =>
                repo.CreateWithClaimAsync(
                    It.Is<Installment>(i =>
                        i.LoanId == _loanId
                        && i.Amount == paymentAmount
                        && i.NextDueDate == nextDueDate
                        && i.Note == "Test payment"
                    )
                ),
            Times.Once
        );
    }

    [Fact()]
    public async Task Handle_WithNullUser_ThrowsForbiddenException()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetUser()).Returns((UserDto?)null);
        var command = new PayInstallmentCommand { };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact()]
    public async Task Handle_WithNonExistentWallet_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new PayInstallmentCommand
        {
            WalletId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };
        _walletRepositoryMock.Setup(repo => repo.GetById(_loanId)).ReturnsAsync((Wallet?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact()]
    public async Task Handle_WithDeletedWallet_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new PayInstallmentCommand
        {
            WalletId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };
        var wallet = new Wallet { Id = _loanId, IsDeleted = true };
        _walletRepositoryMock.Setup(repo => repo.GetById(_loanId)).ReturnsAsync(wallet);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenWalletDoesNotBelongToUser_ThrowsForbiddenException()
    {
        // Arrange
        var command = new PayInstallmentCommand
        {
            LoanId = _loanId,
            WalletId = 1,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };

        var loan = new Loan
        {
            Id = _loanId,
            BorrowerId = _userId,
            DueAmount = 2000m,
            IsDeleted = false,
        };

        var wallet = new Wallet
        {
            Id = command.WalletId,
            UserId = "different-user-id", // Wallet belongs to a different user
            Balance = 1000m,
            IsDeleted = false,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsBorrowerAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        _walletRepositoryMock.Setup(repo => repo.GetById(command.WalletId)).ReturnsAsync(wallet);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenPaymentAmountExceedsDueAmount_ThrowsBadRequestException()
    {
        // Arrange
        var command = new PayInstallmentCommand
        {
            LoanId = _loanId,
            WalletId = 1,
            Amount = 3000m, // Exceeds the loan's due amount
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };

        var loan = new Loan
        {
            Id = _loanId,
            BorrowerId = _userId,
            DueAmount = 2000m, // Loan's due amount is less than the payment amount
            IsDeleted = false,
        };

        var wallet = new Wallet
        {
            Id = command.WalletId,
            UserId = _userId,
            Balance = 5000m,
            IsDeleted = false,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsBorrowerAsync(_loanId, _userId))
            .ReturnsAsync(loan);

        _walletRepositoryMock.Setup(repo => repo.GetById(command.WalletId)).ReturnsAsync(wallet);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
