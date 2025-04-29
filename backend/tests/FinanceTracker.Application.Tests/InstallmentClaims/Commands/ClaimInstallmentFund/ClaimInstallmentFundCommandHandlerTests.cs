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

namespace FinanceTracker.Application.InstallmentClaims.Commands.ClaimInstallmentFund.Tests;

public class ClaimInstallmentFundCommandHandlerTests
{
    private readonly Mock<ILogger<ClaimInstallmentFundCommandHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly ClaimInstallmentFundCommandHandler _handler;
    private readonly string _userId = "test-user-id";
    private readonly int _walletId = 1;
    private readonly int _claimId = 2;
    private readonly int _installmentId = 3;

    public ClaimInstallmentFundCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ClaimInstallmentFundCommandHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _installmentRepositoryMock = new Mock<IInstallmentRepository>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _handler = new ClaimInstallmentFundCommandHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _installmentRepositoryMock.Object,
            _walletRepositoryMock.Object
        );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_UpdatesInstallmentClaimAndWallet()
    {
        // Arrange
        var installmentAmount = 100.0m;

        var user = new UserDto(_userId, "yes@s.com") { };
        var installment = new Installment { Amount = installmentAmount };
        var installmentClaim = new InstallmentClaim
        {
            Id = _claimId,
            InstallmentId = _installmentId,
            IsClaimed = false,
            Installment = installment,
        };
        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId,
            Balance = 50.0m,
        };

        var command = new ClaimInstallmentFundCommand { Id = _claimId, WalletId = _walletId };

        _userContextMock.Setup(x => x.GetUser()).Returns(user);
        _installmentRepositoryMock
            .Setup(x => x.GetInstallmentClaimByIdAsync(_claimId, _userId))
            .ReturnsAsync(installmentClaim);
        _walletRepositoryMock.Setup(x => x.GetById(_walletId)).ReturnsAsync(wallet);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Xunit.Assert.True(installmentClaim.IsClaimed);
        Xunit.Assert.NotNull(installmentClaim.ClaimedAt);
        Xunit.Assert.Equal(150.0m, wallet.Balance); // 50 + 100
        _installmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNullUser_ThrowsForbiddenException()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetUser()).Returns((UserDto?)null);
        var command = new ClaimInstallmentFundCommand { Id = _installmentId, WalletId = _walletId };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WithNonExistentInstallmentClaim_ThrowsNotFoundException()
    {
        // Arrange
        var user = new UserDto(_userId, "tet@g.co");
        _userContextMock.Setup(x => x.GetUser()).Returns(user);

        _installmentRepositoryMock
            .Setup(x => x.GetInstallmentClaimByIdAsync(_claimId, _userId))
            .ReturnsAsync((InstallmentClaim?)null);

        var command = new ClaimInstallmentFundCommand { Id = _claimId, WalletId = _walletId };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact()]
    public async Task Handle_WithAlreadyClaimedInstallment_ThrowsBadRequestException()
    {
        // Arrange
        var installmentClaim = new InstallmentClaim
        {
            Id = _claimId,
            InstallmentId = _installmentId,
            IsClaimed = true,
        };

        var user = new UserDto(_userId, "tet@g.co");
        _userContextMock.Setup(x => x.GetUser()).Returns(user);

        _installmentRepositoryMock
            .Setup(x => x.GetInstallmentClaimByIdAsync(_claimId, _userId))
            .ReturnsAsync(installmentClaim);

        var command = new ClaimInstallmentFundCommand { Id = _claimId, WalletId = _walletId };

        // Act & Assert
        var exception = await Xunit.Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
