using AutoMapper;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Categories.Queries.GetCategoryById.Tests;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetCategoryByIdQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

    private readonly GetCategoryByIdQueryHandler _handler;

    private readonly string _userId = "test-user-id";

    public GetCategoryByIdQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetCategoryByIdQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _handler = new GetCategoryByIdQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _categoryRepositoryMock.Object
        );
    }
    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnValidWalletByIdAsync()
    {
        // Arrange
        var categoryId = 1;
        var command = new GetCategoryByIdQuery()
        {
            Id = categoryId,
        };
        var category = new Category()
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
            UserId = _userId
        };
        var expectedDto = new CategoryDto
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
        };

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId))
            .ReturnsAsync(category);
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);
        _mapperMock.Setup(m => m.Map<CategoryDto>(category))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);
        _mapperMock.Verify(m => m.Map<CategoryDto>(category), Times.Once);
    }
}