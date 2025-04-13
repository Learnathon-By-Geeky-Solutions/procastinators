using AutoMapper;
using Castle.Core.Logging;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceTracker.Application.Wallets.Commands.CreateWallet.Tests;

public class CreateWalletCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedWalletId()
    {
        // Arrange

        var loggerMock = new Mock<ILogger<CreateWalletCommandHandler>>();

        var userContextMock = new Mock<IUserContext>();
        var user = new UserDto("test", "test@test.com");
        userContextMock
            .Setup(u => u.GetUser())
            .Returns(user);

        var mapperMock = new Mock<IMapper>();
        var command = new CreateWalletCommand();
        var wallet = new Wallet();
        mapperMock
            .Setup(m => m.Map<CreateWalletCommand, Wallet>(command))
            .Returns(wallet);

        var walletRepositoryMock = new Mock<IWalletRepository>();
        walletRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<Wallet>()))
            .ReturnsAsync(1);

        var commandHandler = new CreateWalletCommandHandler(loggerMock.Object, userContextMock.Object, mapperMock.Object, walletRepositoryMock.Object);

        // Act

        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert

        result.Should().Be(1);
        wallet.UserId.Should().Be("test");
        walletRepositoryMock.Verify(r => r.Create(wallet), Times.Once);
      
    }
}