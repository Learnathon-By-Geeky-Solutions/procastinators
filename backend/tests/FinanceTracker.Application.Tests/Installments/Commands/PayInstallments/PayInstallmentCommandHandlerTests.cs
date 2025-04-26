using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Installments.Commands.PayInstallments.Tests;

public class PayInstallmentCommandHandlerTests
{
    private readonly Mock<ILogger<PayInstallmentCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly PayInstallmentCommandHandler _handler;
    private readonly string _borrowerId = "borrower-id";
    private readonly string _lenderId = "lender-id";
    private readonly int _loanId = 1;

    public PayInstallmentCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<PayInstallmentCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();
        _handler = new PayInstallmentCommandHandler(
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
        decimal initialLenderBalance = 2000m;
        decimal initialLoanDueAmount = 2000m;

        var loanRequest = new LoanRequest { BorrowerId = _borrowerId, LenderId = _lenderId };

        var loan = new Loan
        {
            Id = _loanId,
            LoanRequest = loanRequest,
            DueAmount = initialLoanDueAmount,
            DueDate = currentDate,
            IsDeleted = false,
        };

        var borrowerWallet = new Wallet
        {
            Id = 1,
            UserId = _borrowerId,
            Balance = initialBorrowerBalance,
            IsDeleted = false,
        };

        var lenderWallet = new Wallet
        {
            Id = 2,
            UserId = _lenderId,
            Balance = initialLenderBalance,
            IsDeleted = false,
        };

        var command = new PayInstallmentCommand
        {
            LoanId = _loanId,
            Amount = paymentAmount,
            NextDueDate = nextDueDate,
            Note = "Test payment",
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
        _loanRepositoryMock.Setup(repo => repo.GetByIdAsync(_loanId)).ReturnsAsync(loan);

        _walletRepositoryMock
            .Setup(repo => repo.GetAll(_borrowerId))
            .ReturnsAsync(new List<Wallet> { borrowerWallet });

        _walletRepositoryMock
            .Setup(repo => repo.GetAll(_lenderId))
            .ReturnsAsync(new List<Wallet> { lenderWallet });

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

        // Verify borrower wallet balance decreased
        Xunit.Assert.Equal(initialBorrowerBalance - paymentAmount, borrowerWallet.Balance);

        // Verify lender wallet balance increased
        Xunit.Assert.Equal(initialLenderBalance + paymentAmount, lenderWallet.Balance);

        // Verify loan due amount decreased
        Xunit.Assert.Equal(initialLoanDueAmount - paymentAmount, loan.DueAmount);

        // Verify loan due date updated
        Xunit.Assert.Equal(nextDueDate, loan.DueDate);

        // Verify installment was created with correct values
        _installmentRepositoryMock.Verify(
            repo =>
                repo.CreateAsync(
                    It.Is<Installment>(i =>
                        i.LoanId == _loanId
                        && i.Amount == paymentAmount
                        && i.NextDueDate == nextDueDate
                        && i.Note == "Test payment"
                    )
                ),
            Times.Once
        );

        // Verify changes were saved
        _loanRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        _installmentRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}
