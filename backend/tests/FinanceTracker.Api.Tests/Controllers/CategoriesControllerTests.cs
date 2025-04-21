using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Categories.Queries.GetAllCategories;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace FinanceTracker.Api.Controllers.Tests;

public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;

    public CategoriesControllerTests(WebApplicationFactory<Program> factory)
    {
        _mediatorMock = new Mock<IMediator>();

        // Set up the mediator mock to return some test data
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<CategoryDto>
                {
                    new CategoryDto { Id = 1, Title = "Food" },
                    new CategoryDto { Id = 2, Title = "Entertainment" },
                }
            );

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                // Replace the real mediator with our mock
                services.AddSingleton<IMediator>(_mediatorMock.Object);

                // Allow unauthorized requests for testing
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            });
        });
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("api/Categories");

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public void GetByIdTest() { }

    [Fact()]
    public void CreateTest() { }

    [Fact()]
    public void DeleteTest() { }

    [Fact()]
    public void UpdateTest() { }
}
