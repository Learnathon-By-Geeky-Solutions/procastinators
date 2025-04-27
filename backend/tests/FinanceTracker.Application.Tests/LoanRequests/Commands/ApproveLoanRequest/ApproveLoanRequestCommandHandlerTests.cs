using System;
using System.Threading;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest.Tests;

public class ApproveLoanRequestCommandHandlerTests
{
    private readonly Mock<ILogger<ApproveLoanRequestCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<ILoanRequestRepository> _loanRequestRepository;
    private readonly ApproveLoanRequestCommandHandler _handler;
    private readonly string _borrowerId = "borrower-id";
    private readonly string _lenderId = "lender-id";
    private readonly int _loanRequestId = 1;
    private readonly int _lenderWalletId = 2;
    private readonly int _borrowerWalletId = 3;

    public ApproveLoanRequestCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ApproveLoanRequestCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _loanRequestRepository = new Mock<ILoanRequestRepository>();
        _handler = new ApproveLoanRequestCommandHandler(
            _loggerMock.Object,
            _loanRequestRepository.Object,
            _loanRepositoryMock.Object,
            _walletRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_ApprovesLoanAndTransfersAmount()
    {
        // Arrange
        decimal loanAmount = 1000m;
        decimal initialLenderBalance = 2000m;
        decimal initialBorrowerBalance = 500m;
        DateTime dueDate = DateTime.UtcNow.AddMonths(1);

        var loanRequest = new LoanRequest
        {
            Id = _loanRequestId,
            BorrowerId = _borrowerId,
            LenderId = _lenderId,
            Amount = loanAmount,
            DueDate = dueDate,
            Note = "Test loan request",
            IsApproved = false,
            WalletId = _borrowerWalletId,
        };

        var lenderWallet = new Wallet
        {
            Id = _lenderWalletId,
            UserId = _lenderId,
            Balance = initialLenderBalance,
        };

        var borrowerWallet = new Wallet
        {
            Id = _borrowerWalletId,
            UserId = _borrowerId,
            Balance = initialBorrowerBalance,
        };

        var command = new ApproveLoanRequestCommand(_loanRequestId, _lenderWalletId)
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Setup mocks
        _loanRequestRepository
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync(loanRequest);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_lenderWalletId))
            .ReturnsAsync(lenderWallet);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_borrowerWalletId))
            .ReturnsAsync(borrowerWallet);

        _loanRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Loan>()))
            .Callback<Loan>(loan => loan.Id = 1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.Equal(1, result); // Loan ID should be returned

        // Verify loan request was marked as approved
        Xunit.Assert.True(loanRequest.IsApproved);

        // Verify wallet balances were updated correctly
        Xunit.Assert.Equal(initialLenderBalance - loanAmount, lenderWallet.Balance);
        Xunit.Assert.Equal(initialBorrowerBalance + loanAmount, borrowerWallet.Balance);

        // Verify loan was created with correct properties
        _loanRepositoryMock.Verify(
            repo =>
                repo.CreateAsync(
                    It.Is<Loan>(loan =>
                        loan.LenderId == _lenderId
                        && loan.LoanRequestId == _loanRequestId
                        && loan.Amount == loanAmount
                        && loan.Note == "Test loan request"
                        && loan.DueDate == dueDate
                        && loan.DueAmount == loanAmount
                        && loan.WalletId == _lenderWalletId
                        && loan.BorrowerWalletId == _borrowerWalletId
                    )
                ),
            Times.Once
        );

        // Verify changes were saved
        _loanRequestRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_LoanRequestNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _loanRequestRepository
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync((LoanRequest?)null);

        var command = new ApproveLoanRequestCommand(_loanRequestId, _lenderWalletId)
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        Xunit.Assert.Contains(_loanRequestId.ToString(), exception.Message);
    }

    [Fact]
    public async Task Handle_AlreadyApprovedLoanRequest_ThrowsBadRequestException()
    {
        // Arrange
        var loanRequest = new LoanRequest { Id = _loanRequestId, IsApproved = true };

        _loanRequestRepository
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync(loanRequest);

        var command = new ApproveLoanRequestCommand(_loanRequestId, _lenderWalletId)
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        Xunit.Assert.Contains("already approved", exception.Message);
    }

    [Fact]
    public async Task Handle_NullWalletId_ThrowsBadRequestException()
    {
        // Arrange
        var loanRequest = new LoanRequest
        {
            Id = _loanRequestId,
            IsApproved = false,
            WalletId = null,
        };

        _loanRequestRepository
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync(loanRequest);

        var command = new ApproveLoanRequestCommand(_loanRequestId, _lenderWalletId)
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        Xunit.Assert.Contains("WalletId cannot be null", exception.Message);
    }

    [Fact]
    public async Task Handle_LenderWalletNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var loanRequest = new LoanRequest
        {
            Id = _loanRequestId,
            IsApproved = false,
            WalletId = _borrowerWalletId,
            LenderId = _lenderId,
        };

        _loanRequestRepository
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync(loanRequest);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_lenderWalletId))
            .ReturnsAsync((Wallet?)null);

        var command = new ApproveLoanRequestCommand(_loanRequestId, _lenderWalletId)
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        Xunit.Assert.Contains(_lenderId.ToString(), exception.Message);
    }

    [Fact]
    public async Task Handle_BorrowerWalletNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var loanRequest = new LoanRequest
        {
            Id = _loanRequestId,
            BorrowerId = _borrowerId,
            LenderId = _lenderId,
            IsApproved = false,
            WalletId = _borrowerWalletId,
        };

        var lenderWallet = new Wallet { Id = _lenderWalletId, UserId = _lenderId };

        _loanRequestRepository
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync(loanRequest);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_lenderWalletId))
            .ReturnsAsync(lenderWallet);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_borrowerWalletId))
            .ReturnsAsync((Wallet?)null);

        var command = new ApproveLoanRequestCommand(_loanRequestId, _lenderWalletId)
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        Xunit.Assert.Contains(_borrowerId.ToString(), exception.Message);
    }
}
