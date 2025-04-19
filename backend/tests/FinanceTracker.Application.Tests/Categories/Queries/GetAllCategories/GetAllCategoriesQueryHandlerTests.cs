using AutoMapper;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Application.Wallets.Queries.GetAllWallets;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Categories.Queries.GetAllCategories.Tests;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllCategoriesQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly GetAllCategoriesQueryHandler _handler;
    private readonly string _userId = "test-user-id";

    public GetAllCategoriesQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllCategoriesQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _handler = new GetAllCategoriesQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _categoryRepositoryMock.Object
        );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldReturnAllCategoriesForCurrentUser()
    {
        // Arrange
        var query = new GetAllCategoriesQuery();

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var categories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Title = "test",
                DefaultTransactionType = "Income",
                UserId = _userId
            },
            new Category
            {
                Id = 2,
                Title = "test2",
                DefaultTransactionType = "Expense",
                UserId = _userId
            }
        };

        _categoryRepositoryMock.Setup(r => r.GetAll(_userId))
            .ReturnsAsync(categories);

        var categoryDtos = new List<CategoryDto>
        {
            new CategoryDto
            {
                Id = 1,
                Title = "test",
                DefaultTransactionType = "Income",
            },
            new CategoryDto
            {
                Id = 2,
                Title = "test2",
                DefaultTransactionType = "Expense",
            }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<CategoryDto>>(categories))
            .Returns(categoryDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(categoryDtos);
        _categoryRepositoryMock.Verify(r => r.GetAll(_userId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<CategoryDto>>(categories), Times.Once);
    }
}