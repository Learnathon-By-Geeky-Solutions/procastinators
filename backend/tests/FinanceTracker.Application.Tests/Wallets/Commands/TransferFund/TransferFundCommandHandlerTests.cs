using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Wallets.Commands.TransferFund.Tests;

public class TransferFundCommandHandlerTests
{
    private readonly Mock<ILogger<TransferFundCommandHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;

    private readonly TransferFundCommandHandler _handler;

    private readonly string _userId = "test-user-id";

    public TransferFundCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<TransferFundCommandHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _handler = new TransferFundCommandHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _walletRepositoryMock.Object
        );
    }
    [Fact]
    public async Task Handle_WithValidRequest_ShouldTransferFundsBetweenWallets()
    {
        // Arrange
        var sourceWalletId = 1;
        var destinationWalletId = 2;
        var transferAmount = 100m;

        var command = new TransferFundCommand
        {
            SourceWalletId = sourceWalletId,
            DestinationWalletId = destinationWalletId,
            Amount = transferAmount
        };

        var sourceWallet = new Wallet
        {
            Id = sourceWalletId,
            Name = "Source-Wallet",
            Balance = 500m,
            UserId = _userId
        };

        var destinationWallet = new Wallet
        {
            Id = destinationWalletId,
            Name = "Destination-Wallet",
            Balance = 200m,
            UserId = _userId
        };

        _walletRepositoryMock.Setup(r => r.GetById(sourceWalletId))
            .ReturnsAsync(sourceWallet);
        _walletRepositoryMock.Setup(r => r.GetById(destinationWalletId))
            .ReturnsAsync(destinationWallet);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sourceWallet.Balance.Should().Be(400m);
        destinationWallet.Balance.Should().Be(300m);
        _walletRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    [Fact]
    public async Task Handle_SourceWalletNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var sourceWalletId = 1;
        var destinationWalletId = 2;
        var transferAmount = 100m;

        var command = new TransferFundCommand
        {
            SourceWalletId = sourceWalletId,
            DestinationWalletId = destinationWalletId,
            Amount = transferAmount
        };

        _walletRepositoryMock.Setup(r => r.GetById(sourceWalletId))
            .ReturnsAsync((Wallet?)null);

        var destinationWallet = new Wallet
        {
            Id = destinationWalletId,
            Name = "Destination Wallet",
            Balance = 200m,
            UserId = _userId
        };

        _walletRepositoryMock.Setup(r => r.GetById(destinationWalletId))
            .ReturnsAsync(destinationWallet);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DestinationWalletNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var sourceWalletId = 1;
        var destinationWalletId = 2;
        var transferAmount = 100m;

        var command = new TransferFundCommand
        {
            SourceWalletId = sourceWalletId,
            DestinationWalletId = destinationWalletId,
            Amount = transferAmount
        };

        var sourceWallet = new Wallet
        {
            Id = sourceWalletId,
            Name = "Source Wallet",
            Balance = 500m,
            UserId = _userId
        };

        _walletRepositoryMock.Setup(r => r.GetById(sourceWalletId))
            .ReturnsAsync(sourceWallet);
        _walletRepositoryMock.Setup(r => r.GetById(destinationWalletId))
            .ReturnsAsync((Wallet?)null);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Handle_NullUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var sourceWalletId = 1;
        var destinationWalletId = 2;
        var transferAmount = 100m;

        var command = new TransferFundCommand
        {
            SourceWalletId = sourceWalletId,
            DestinationWalletId = destinationWalletId,
            Amount = transferAmount
        };

        var sourceWallet = new Wallet
        {
            Id = sourceWalletId,
            Name = "Source Wallet",
            Balance = 500m,
            UserId = _userId
        };

        var destinationWallet = new Wallet
        {
            Id = destinationWalletId,
            Name = "Destination Wallet",
            Balance = 200m,
            UserId = _userId
        };

        _walletRepositoryMock.Setup(r => r.GetById(sourceWalletId))
            .ReturnsAsync(sourceWallet);
        _walletRepositoryMock.Setup(r => r.GetById(destinationWalletId))
            .ReturnsAsync(destinationWallet);

        // Setup null user
        _userContextMock.Setup(u => u.GetUser())
            .Returns((UserDto?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SourceWalletBelongsToDifferentUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var sourceWalletId = 1;
        var destinationWalletId = 2;
        var transferAmount = 100m;

        var command = new TransferFundCommand
        {
            SourceWalletId = sourceWalletId,
            DestinationWalletId = destinationWalletId,
            Amount = transferAmount
        };

        var sourceWallet = new Wallet
        {
            Id = sourceWalletId,
            Name = "Source Wallet",
            Balance = 500m,
            UserId = "different-user-id"  // Different user ID
        };

        var destinationWallet = new Wallet
        {
            Id = destinationWalletId,
            Name = "Destination Wallet",
            Balance = 200m,
            UserId = _userId
        };

        _walletRepositoryMock.Setup(r => r.GetById(sourceWalletId))
            .ReturnsAsync(sourceWallet);
        _walletRepositoryMock.Setup(r => r.GetById(destinationWalletId))
            .ReturnsAsync(destinationWallet);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DestinationWalletBelongsToDifferentUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var sourceWalletId = 1;
        var destinationWalletId = 2;
        var transferAmount = 100m;

        var command = new TransferFundCommand
        {
            SourceWalletId = sourceWalletId,
            DestinationWalletId = destinationWalletId,
            Amount = transferAmount
        };

        var sourceWallet = new Wallet
        {
            Id = sourceWalletId,
            Name = "Source Wallet",
            Balance = 500m,
            UserId = _userId
        };

        var destinationWallet = new Wallet
        {
            Id = destinationWalletId,
            Name = "Destination Wallet",
            Balance = 200m,
            UserId = "different-user-id"  // Different user ID
        };

        _walletRepositoryMock.Setup(r => r.GetById(sourceWalletId))
            .ReturnsAsync(sourceWallet);
        _walletRepositoryMock.Setup(r => r.GetById(destinationWalletId))
            .ReturnsAsync(destinationWallet);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}