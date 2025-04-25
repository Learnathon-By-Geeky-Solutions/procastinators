using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetReportOnCategories.Tests;

public class GetReportOnCategoriesQueryHandlerTests
{
    private readonly Mock<ILogger<GetReportOnCategoriesQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPersonalTransactionRepository> _transactionRepositoryMock;
    private readonly GetReportOnCategoriesQueryHandler _handler;
    private readonly string _userId = "test-user-id";

    public GetReportOnCategoriesQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetReportOnCategoriesQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _transactionRepositoryMock = new Mock<IPersonalTransactionRepository>();
        _handler = new GetReportOnCategoriesQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _transactionRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnReportOnCategories()
    {
        // Arrange
        var query = new GetReportOnCategoriesQuery { Days = 30, Type = "expense" };

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        // Create proper Category entities
        var categories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Title = "Food",
                UserId = _userId,
                DefaultTransactionType = "expense",
            },
            new Category
            {
                Id = 2,
                Title = "Transportation",
                UserId = _userId,
                DefaultTransactionType = "expense",
            },
            new Category
            {
                Id = 3,
                Title = "Entertainment",
                UserId = _userId,
                DefaultTransactionType = "expense",
            },
        };

        // Create category-total data that matches what your repository would return
        var categoryTotals = new List<TotalPerCategory>
        {
            new TotalPerCategory(categories[0], 150.50m),
            new TotalPerCategory(categories[1], 75.25m),
            new TotalPerCategory(categories[2], 50.00m),
        };

        decimal grandTotal = 275.75m;

        // Setup repository with the correct return type
        _transactionRepositoryMock
            .Setup(r => r.GetReportOnCategories(_userId, query.Days, query.Type))
            .Returns(
                Task.FromResult((categoryTotals as IEnumerable<TotalPerCategory>, grandTotal))
            );

        // Define the expected mapped result
        var expectedCategories = new List<TotalPerCategoryDto>
        {
            new TotalPerCategoryDto
            {
                CategoryId = 1,
                CategoryTitle = "Food",
                Total = 150.50m,
            },
            new TotalPerCategoryDto
            {
                CategoryId = 2,
                CategoryTitle = "Transportation",
                Total = 75.25m,
            },
            new TotalPerCategoryDto
            {
                CategoryId = 3,
                CategoryTitle = "Entertainment",
                Total = 50.00m,
            },
        };

        _mapperMock
            .Setup(m => m.Map<IEnumerable<TotalPerCategoryDto>>(categoryTotals))
            .Returns(expectedCategories);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.GrandTotal.Should().Be(grandTotal);
        result.Categories.Should().BeEquivalentTo(expectedCategories);
        _transactionRepositoryMock.Verify(
            r => r.GetReportOnCategories(_userId, query.Days, query.Type),
            Times.Once
        );
        _mapperMock.Verify(
            m => m.Map<IEnumerable<TotalPerCategoryDto>>(categoryTotals),
            Times.Once
        );
    }
}
