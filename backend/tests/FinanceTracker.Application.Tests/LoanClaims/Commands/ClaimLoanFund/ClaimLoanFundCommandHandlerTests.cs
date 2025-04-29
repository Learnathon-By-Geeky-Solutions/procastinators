using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.LoanClaims.Commands.ClaimLoanFund.Tests;

public class ClaimLoanFundCommandHandlerTests
{
    private readonly Mock<ILogger<ClaimLoanFundCommandHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly ClaimLoanFundCommandHandler _handler;
    private readonly string _userId = "test-user-id";
    private readonly int _walletId = 1;
    private readonly int _claimId = 2;
    private readonly int _LoanId = 3;

    public ClaimLoanFundCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ClaimLoanFundCommandHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _handler = new ClaimLoanFundCommandHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _loanRepositoryMock.Object,
            _walletRepositoryMock.Object
        );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ClaimLoanFund()
    {
        // Arrange
        var LoanAmount = 100.0m;

        var user = new UserDto(_userId, "yes@s.com") { };
        var Loan = new Loan { Amount = LoanAmount };
        var LoanClaim = new LoanClaim
        {
            Id = _claimId,
            LoanId = _LoanId,
            IsClaimed = false,
            Loan = Loan,
        };
        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId,
            Balance = 50.0m,
        };

        var command = new ClaimLoanFundCommand { Id = _claimId, WalletId = _walletId };

        _userContextMock.Setup(x => x.GetUser()).Returns(user);
        _loanRepositoryMock
            .Setup(x => x.GetLoanClaimByIdAsync(_claimId, _userId))
            .ReturnsAsync(LoanClaim);
        _walletRepositoryMock.Setup(x => x.GetById(_walletId)).ReturnsAsync(wallet);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.True(LoanClaim.IsClaimed);
        Xunit.Assert.NotNull(LoanClaim.ClaimedAt);
        Xunit.Assert.Equal(150.0m, wallet.Balance); // 50 + 100
        _loanRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNullUser_ThrowsForbiddenException()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetUser()).Returns((UserDto?)null);
        var command = new ClaimLoanFundCommand { Id = _LoanId, WalletId = _walletId };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithNonExistentLoanClaim_ThrowsNotFoundException()
    {
        // Arrange
        var user = new UserDto(_userId, "tet@g.co");
        _userContextMock.Setup(x => x.GetUser()).Returns(user);

        _loanRepositoryMock
            .Setup(x => x.GetLoanClaimByIdAsync(_claimId, _userId))
            .ReturnsAsync((LoanClaim?)null);

        var command = new ClaimLoanFundCommand { Id = _claimId, WalletId = _walletId };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact()]
    public async Task Handle_WithAlreadyClaimedLoan_ThrowsBadRequestException()
    {
        // Arrange
        var LoanClaim = new LoanClaim
        {
            Id = _claimId,
            LoanId = _LoanId,
            IsClaimed = true,
        };

        var user = new UserDto(_userId, "tet@g.co");
        _userContextMock.Setup(x => x.GetUser()).Returns(user);

        _loanRepositoryMock
            .Setup(x => x.GetLoanClaimByIdAsync(_claimId, _userId))
            .ReturnsAsync(LoanClaim);

        var command = new ClaimLoanFundCommand { Id = _claimId, WalletId = _walletId };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithNonExistentWallet_ThrowsNotFoundException()
    {
        // Arrange
        var user = new UserDto(_userId, "user@example.com");
        var LoanClaim = new LoanClaim
        {
            Id = _claimId,
            LoanId = _LoanId,
            IsClaimed = false,
        };

        _userContextMock.Setup(x => x.GetUser()).Returns(user);
        _loanRepositoryMock
            .Setup(x => x.GetLoanClaimByIdAsync(_claimId, _userId))
            .ReturnsAsync(LoanClaim);
        _walletRepositoryMock.Setup(x => x.GetById(_walletId)).ReturnsAsync((Wallet?)null);

        var command = new ClaimLoanFundCommand { Id = _claimId, WalletId = _walletId };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithWalletBelongingToDifferentUser_ThrowsForbiddenException()
    {
        // Arrange
        var differentUserId = "2";
        var user = new UserDto(_userId, "user@example.com");
        var LoanClaim = new LoanClaim
        {
            Id = _claimId,
            LoanId = _LoanId,
            IsClaimed = false,
        };
        var wallet = new Wallet { Id = _walletId, UserId = differentUserId };

        _userContextMock.Setup(x => x.GetUser()).Returns(user);
        _loanRepositoryMock
            .Setup(x => x.GetLoanClaimByIdAsync(_claimId, _userId))
            .ReturnsAsync(LoanClaim);
        _walletRepositoryMock.Setup(x => x.GetById(_walletId)).ReturnsAsync(wallet);

        var command = new ClaimLoanFundCommand { Id = _claimId, WalletId = _walletId };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
