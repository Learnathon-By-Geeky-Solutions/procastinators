using AutoMapper;
using Castle.Core.Logging;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.CreateWallet;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceTracker.Application.Categories.Commands.CreateCategory.Tests;

public class CreateCategoryCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedWalletId()
    {
        // Arrange

        var loggerMock = new Mock<ILogger<CreateCategoryCommandHandler>>();

        var userContextMock = new Mock<IUserContext>();
        var user = new UserDto("test", "test@test.com");
        userContextMock
            .Setup(u => u.GetUser())
            .Returns(user);

        var mapperMock = new Mock<IMapper>();
        var command = new CreateCategoryCommand();
        var category = new Category();
        mapperMock
            .Setup(m => m.Map<CreateCategoryCommand, Category>(command))
            .Returns(category);

        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        categoryRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<Category>()))
            .ReturnsAsync(1);

        var commandHandler = new CreateCategoryCommandHandler(loggerMock.Object, userContextMock.Object, mapperMock.Object, categoryRepositoryMock.Object);

        // Act

        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert

        result.Should().Be(1);
        category.UserId.Should().Be("test");
        categoryRepositoryMock.Verify(r => r.Create(category), Times.Once);

    }
}