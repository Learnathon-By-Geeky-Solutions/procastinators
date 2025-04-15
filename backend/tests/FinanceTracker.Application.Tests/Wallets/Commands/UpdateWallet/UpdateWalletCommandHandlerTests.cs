using AutoMapper;
using FinanceTracker.Application.Categories.Commands.UpdateCategory;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Wallets.Commands.UpdateWallet.Tests;

public class UpdateWalletCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateWalletCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly UpdateWalletCommandHandler _handler;

    private readonly string _userId = "test-user-id";

    public UpdateWalletCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateWalletCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new UpdateWalletCommandHandler(
                _loggerMock.Object,
                _userContextMock.Object,
                _mapperMock.Object,
                _walletRepositoryMock.Object
            );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateWallet()
    {
        // Arrange

        var walletId = 1;
        var command = new UpdateWalletCommand()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT"
        };

        var wallet = new Wallet()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT",
            UserId = _userId
        };

        _walletRepositoryMock.Setup(r => r.GetById(walletId))
            .ReturnsAsync(wallet);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Assert

        _walletRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, wallet), Times.Once);

    }
    [Fact()]
    public async Task Handle_WithNonExistentWallet_ShouldThrowNotFoundException()
    {
        // Arrange
        var walletId = 1;
        var command = new UpdateWalletCommand()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT"
        };

        _walletRepositoryMock.Setup(r => r.GetById(walletId))
            .ReturnsAsync((Wallet?)null);

        var user = new UserDto("test", "test@test.com") { Id = _userId };

        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _walletRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact()]
    public async Task Handle_WithDeletedWallet_ShouldThrowNotFoundException()
    {
        // Arrange
        var walletId = 1;
        var command = new UpdateWalletCommand()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT"
        };

        var wallet = new Wallet()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT",
            UserId = _userId,
            IsDeleted = true
        };

        _walletRepositoryMock.Setup(r => r.GetById(walletId))
            .ReturnsAsync(wallet);

        var user = new UserDto("test", "test@test.com") { Id = _userId };

        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _walletRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact()]
    public async Task Handle_WithWalletBelongingToAnotherUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var walletId = 1;
        var command = new UpdateWalletCommand()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT"
        };

        var wallet = new Wallet()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT",
            UserId = "different-user-id",
        };

        _walletRepositoryMock.Setup(r => r.GetById(walletId))
            .ReturnsAsync(wallet);

        var user = new UserDto("test", "test@test.com") { Id = _userId };

        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _walletRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}