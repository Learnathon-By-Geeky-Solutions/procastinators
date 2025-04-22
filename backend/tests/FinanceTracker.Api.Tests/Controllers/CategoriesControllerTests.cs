using System.Net.Http.Json;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Categories.Commands.DeleteCategory;
using FinanceTracker.Application.Categories.Commands.UpdateCategory;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Categories.Queries.GetAllCategories;
using FinanceTracker.Application.Categories.Queries.GetCategoryById;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace FinanceTracker.Api.Controllers.Tests;

public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();

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

                services.Replace(
                    ServiceDescriptor.Scoped(
                        typeof(ICategoryRepository),
                        _ => _categoryRepositoryMock.Object
                    )
                );
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
    public async Task GetById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 11;
        var categoryDto = new CategoryDto { Id = id, Title = "test" };

        _mediatorMock
            .Setup(m =>
                m.Send(It.Is<GetCategoryByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(categoryDto);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Categories/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m =>
                m.Send(It.Is<GetCategoryByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task Create_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var createCommand = new CreateCategoryCommand
        {
            Title = "New Category",
            DefaultTransactionType = "Expense",
        };
        int createdId = 42;

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdId);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/Categories", createCommand);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.ToString().Should().Contain($"/api/Categories/{createdId}");
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task Delete_WithExistingId_ReturnsNoContent()
    {
        // Arrange
        var id = 5;

        _mediatorMock
            .Setup(m =>
                m.Send(It.Is<DeleteCategoryCommand>(c => c.Id == id), It.IsAny<CancellationToken>())
            )
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"api/Categories/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<DeleteCategoryCommand>(c => c.Id == id),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact()]
    public async Task Update_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var id = 10;
        var updateCommand = new UpdateCategoryCommand
        {
            Title = "Updated Category",
            DefaultTransactionType = "Income",
        };

        _mediatorMock
            .Setup(m =>
                m.Send(It.Is<UpdateCategoryCommand>(c => c.Id == id), It.IsAny<CancellationToken>())
            )
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PatchAsync(
            $"api/Categories/{id}",
            JsonContent.Create(updateCommand)
        );

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<UpdateCategoryCommand>(c => c.Id == id),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
