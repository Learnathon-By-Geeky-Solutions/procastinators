using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Application.Installments.Commands.PayInstallment;
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
    public async Task Handle_WithNonExistentLoan_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new PayInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_loanId, _userId))
            .ReturnsAsync((Loan?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact()]
    public async Task Handle_WithDeletedLoan_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new PayInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
            Note = "Test payment",
        };
        var loan = new Loan { Id = _loanId, IsDeleted = true };
        _loanRepositoryMock.Setup(repo => repo.GetByIdAsync(_loanId, _userId)).ReturnsAsync(loan);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_BorrowerWalletNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var loanRequest = new LoanRequest { BorrowerId = _borrowerId, LenderId = _lenderId };

        var loan = new Loan
        {
            Id = _loanId,
            LoanRequest = loanRequest,
            IsDeleted = false,
        };

        _loanRepositoryMock.Setup(repo => repo.GetByIdAsync(_loanId, _userId)).ReturnsAsync(loan);

        _walletRepositoryMock
            .Setup(repo => repo.GetAll(_borrowerId))
            .ReturnsAsync(new List<Wallet>()); // Empty list

        var command = new PayInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_LenderWalletNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var loanRequest = new LoanRequest { BorrowerId = _borrowerId, LenderId = _lenderId };

        var loan = new Loan
        {
            Id = _loanId,
            LoanRequest = loanRequest,
            IsDeleted = false,
        };

        var borrowerWallet = new Wallet { UserId = _borrowerId, IsDeleted = false };

        _loanRepositoryMock.Setup(repo => repo.GetByIdAsync(_loanId, _userId)).ReturnsAsync(loan);

        _walletRepositoryMock
            .Setup(repo => repo.GetAll(_borrowerId))
            .ReturnsAsync(new List<Wallet> { borrowerWallet });

        _walletRepositoryMock
            .Setup(repo => repo.GetAll(_lenderId))
            .ReturnsAsync(new List<Wallet>()); // Empty list

        var command = new PayInstallmentCommand
        {
            LoanId = _loanId,
            Amount = 500m,
            NextDueDate = DateTime.Now.AddMonths(1),
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
