﻿using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Categories.Commands.UpdateCategory.Tests;

public class UpdateCategoryCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateCategoryCommandHandler>> _loggerMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly UpdateCategoryCommandHandler _handler;

    private readonly string _userId = "test-user-id";

    public UpdateCategoryCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateCategoryCommandHandler>>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new UpdateCategoryCommandHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _categoryRepositoryMock.Object
        );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateCategory()
    {
        // Arrange

        var categoryId = 1;
        var command = new UpdateCategoryCommand()
        {
            Id = categoryId,
            Title = "My test",
            DefaultTransactionType = "Income",
        };

        var category = new Category()
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
            UserId = _userId,
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId)).ReturnsAsync(category);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Assert

        _categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, category), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNonExistentCategory_ShouldThrowNotFoundException()
    {
        // Arrange
        var categoryId = 1;
        var command = new UpdateCategoryCommand()
        {
            Id = categoryId,
            Title = "My test",
            DefaultTransactionType = "Income",
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId)).ReturnsAsync((Category?)null);

        var user = new UserDto("test", "test@test.com") { Id = _userId };

        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        _categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact()]
    public async Task Handle_WithDeletedCategory_ShouldThrowNotFoundException()
    {
        // Arrange
        var categoryId = 1;
        var command = new UpdateCategoryCommand()
        {
            Id = categoryId,
            Title = "My test",
            DefaultTransactionType = "Income",
        };

        var category = new Category()
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
            UserId = _userId,
            IsDeleted = true,
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId)).ReturnsAsync(category);

        var user = new UserDto("test", "test@test.com") { Id = _userId };

        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        _categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact()]
    public async Task Handle_WithCategoryBelongingToAnotherUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var categoryId = 1;
        var command = new UpdateCategoryCommand()
        {
            Id = categoryId,
            Title = "My test",
            DefaultTransactionType = "Income",
        };

        var category = new Category()
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
            UserId = "different-user-id", // Different from current user
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId)).ReturnsAsync(category);

        var user = new UserDto("test", "test@test.com") { Id = _userId };

        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        _categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
