using System;
using System.Threading;
using System.Threading.Tasks;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest.Tests;

public class ApproveLoanRequestCommandHandlerTests
{
    private readonly Mock<ILogger<ApproveLoanRequestCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<ILoanRequestRepository> _loanRequestRepository;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly ApproveLoanRequestCommandHandler _handler;
    private readonly string _borrowerId = "borrower-id";
    private readonly string _lenderId = "lender-id";
    private readonly int _loanRequestId = 1;
    private readonly int _lenderWalletId = 2;
    private readonly int _borrowerWalletId = 3;
    private readonly string _userId = "user-id";
    private readonly UserDto? _user;

    public ApproveLoanRequestCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ApproveLoanRequestCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _loanRequestRepository = new Mock<ILoanRequestRepository>();
        _userContextMock = new Mock<IUserContext>();
        _user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);
        _handler = new ApproveLoanRequestCommandHandler(
            _loggerMock.Object,
            _loanRequestRepository.Object,
            _loanRepositoryMock.Object,
            _walletRepositoryMock.Object,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_ApprovesLoanAndTransfersAmount()
    {
        // Arrange
        decimal loanAmount = 1000m;
        decimal initialLenderBalance = 2000m;
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
        };

        var lenderWallet = new Wallet
        {
            Id = _lenderWalletId,
            UserId = _userId,
            Balance = initialLenderBalance,
        };

        var command = new ApproveLoanRequestCommand()
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Setup mocks
        _loanRequestRepository
            .Setup(repo => repo.GetReceivedByIdAsync(_loanRequestId, _userId))
            .ReturnsAsync(loanRequest);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_lenderWalletId))
            .ReturnsAsync(lenderWallet);

        _loanRepositoryMock
            .Setup(repo => repo.CreateWithClaimAsync(It.IsAny<Loan>()))
            .Callback<Loan>(loan => loan.Id = 1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.Equal(1, result); // Loan ID should be returned

        // Verify loan request was marked as approved
        Xunit.Assert.True(loanRequest.IsApproved);

        // Verify wallet balances were updated correctly
        Xunit.Assert.Equal(initialLenderBalance - loanAmount, lenderWallet.Balance);

        // Verify loan was created with correct properties
        _loanRepositoryMock.Verify(
            repo =>
                repo.CreateWithClaimAsync(
                    It.Is<Loan>(loan =>
                        loan.LenderId == _lenderId
                        && loan.LoanRequestId == _loanRequestId
                        && loan.Amount == loanAmount
                        && loan.Note == "Test loan request"
                        && loan.DueDate == dueDate
                        && loan.DueAmount == loanAmount
                        && loan.BorrowerId == _borrowerId
                    )
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_LoanRequestNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _loanRequestRepository
            .Setup(repo => repo.GetReceivedByIdAsync(_loanRequestId, _userId))
            .ReturnsAsync((LoanRequest?)null);

        var command = new ApproveLoanRequestCommand()
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
            .Setup(repo => repo.GetReceivedByIdAsync(_loanRequestId, _userId))
            .ReturnsAsync(loanRequest);

        var command = new ApproveLoanRequestCommand()
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
    public async Task Handle_LenderWalletNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var loanRequest = new LoanRequest
        {
            Id = _loanRequestId,
            IsApproved = false,
            LenderId = _lenderId,
        };

        _loanRequestRepository
            .Setup(repo => repo.GetReceivedByIdAsync(_loanRequestId, _userId))
            .ReturnsAsync(loanRequest);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_lenderWalletId))
            .ReturnsAsync((Wallet?)null);

        var command = new ApproveLoanRequestCommand()
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_LenderWalletNotOwnedByUser_ThrowsForbiddenException()
    {
        // Arrange
        var loanRequest = new LoanRequest
        {
            Id = _loanRequestId,
            LenderId = _lenderId,
            IsApproved = false,
        };

        var lenderWallet = new Wallet
        {
            Id = _lenderWalletId,
            UserId = "different-user-id", // Wallet does not belong to the user
        };

        var testUser = new UserDto(_userId, "test@example.com");
        _userContextMock.Setup(context => context.GetUser()).Returns(testUser);

        _loanRequestRepository
            .Setup(repo => repo.GetReceivedByIdAsync(_loanRequestId, _userId))
            .ReturnsAsync(loanRequest);

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_lenderWalletId))
            .ReturnsAsync(lenderWallet);

        var command = new ApproveLoanRequestCommand
        {
            LoanRequestId = _loanRequestId,
            LenderWalletId = _lenderWalletId,
        };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
