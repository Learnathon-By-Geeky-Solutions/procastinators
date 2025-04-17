using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Wallets.Commands.DeleteWallet.Tests;

public class DeleteWalletCommandHandlerTests
{
    private readonly Mock<ILogger<DeleteWalletCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly DeleteWalletCommandHandler _handler;

    private readonly string _userId = "test-user-id";

    public DeleteWalletCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteWalletCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new DeleteWalletCommandHandler(
                _loggerMock.Object,
                _userContextMock.Object,
                _walletRepositoryMock.Object
            );
    }
    [Fact()]
    public async Task Handle_WithValidRequest_ShouldDeleteWallet()
    {
        // Arrange

        var walletId = 1;
        var command = new DeleteWalletCommand()
        {
            Id = walletId
        };

        var wallet = new Wallet()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT",
            UserId = _userId,
            IsDeleted = false
        };

        _walletRepositoryMock.Setup(r => r.GetById(walletId))
            .ReturnsAsync(wallet);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Assert

        wallet.IsDeleted.Should().BeTrue();
        _walletRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}