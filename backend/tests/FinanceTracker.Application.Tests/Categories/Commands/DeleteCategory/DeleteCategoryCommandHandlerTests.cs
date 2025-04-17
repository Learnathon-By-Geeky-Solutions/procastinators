using AutoMapper;
using FinanceTracker.Application.Categories.Commands.UpdateCategory;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Categories.Commands.DeleteCategory.Tests;

public class DeleteCategoryCommandHandlerTests
{
    private readonly Mock<ILogger<DeleteCategoryCommandHandler>> _loggerMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly DeleteCategoryCommandHandler _handler;

    private readonly string _userId = "test-user-id";

    public DeleteCategoryCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeleteCategoryCommandHandler>>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new DeleteCategoryCommandHandler(
                _loggerMock.Object,
                _userContextMock.Object,
                _categoryRepositoryMock.Object
            );
    }


    [Fact()]
    public async Task Handle_WithValidRequest_ShouldDeleteCategoryAsync()
    {
        // Arrange

        var categoryId = 1;
        var command = new DeleteCategoryCommand()
        {
            Id = categoryId,
        };

        var category = new Category()
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
            UserId = _userId,
            IsDeleted = false
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId))
            .ReturnsAsync(category);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Assert

        category.IsDeleted.Should().BeTrue();
        _categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNonExistentCategory_ShouldThrowNotFoundException()
    {
        // Arrange
        var categoryId = 1;
        var command = new DeleteCategoryCommand()
        {
            Id = categoryId
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId))
            .ReturnsAsync((Category?)null);

        var user = new UserDto("test", "test@test.com") { Id = _userId };

        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact()]
    public async Task Handle_WithCategoryAlreadyDeleted_ShouldThrowNotFoundException()
    {
        // Arrange
        var categoryId = 1;
        var command = new DeleteCategoryCommand()
        {
            Id = categoryId
        };

        var category = new Category()
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
            UserId = _userId,
            IsDeleted = true
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId))
            .ReturnsAsync(category);

        var user = new UserDto("test", "test@test.com") { Id = _userId };

        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}