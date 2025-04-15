﻿using AutoMapper;
using Castle.Core.Logging;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace FinanceTracker.Application.Categories.Commands.UpdateCategory.Tests;

public class UpdateCategoryCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateWalletCommandHandler>> _loggerMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly UpdateCategoryCommandHandler _handler;

    public UpdateCategoryCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateWalletCommandHandler>>();
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
        var userId = "test-user-id";
        var command = new UpdateCategoryCommand()
        {
            Id = categoryId,
            Title = "My test",
            DefaultTransactionType = "Income"
        };

        var category = new Category()
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
            UserId = userId
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId))
            .ReturnsAsync(category);

        var user = new UserDto("test", "test@test.com") { Id = userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Assert

        _categoryRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, category), Times.Once);

    }
}