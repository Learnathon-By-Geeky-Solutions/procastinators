using System.Net.Http.Json;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Categories.Commands.DeleteCategory;
using FinanceTracker.Application.Categories.Commands.UpdateCategory;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Categories.Queries.GetAllCategories;
using FinanceTracker.Application.Categories.Queries.GetCategoryById;
using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Commands.DeletePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;
using FinanceTracker.Application.PersonalTransactions.Queries.GetPersonalTransactionById;
using FinanceTracker.Application.PersonalTransactions.Queries.GetReportOnCategories;
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

public class PersonalTransactionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPersonalTransactionRepository> _personalTransactionRepositoryMock =
        new();

    public PersonalTransactionsControllerTests(WebApplicationFactory<Program> factory)
    {
        _mediatorMock = new Mock<IMediator>();

        // Set up the mediator mock to return some test data
        _mediatorMock
            .Setup(m =>
                m.Send(It.IsAny<GetAllPersonalTransactionsQuery>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(
                new List<PersonalTransactionDto>
                {
                    new PersonalTransactionDto
                    {
                        Id = 1,
                        TransactionType = "Income",
                        Amount = 100,
                    },
                    new PersonalTransactionDto
                    {
                        Id = 2,
                        TransactionType = "Expense",
                        Amount = 110,
                    },
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
                        typeof(IPersonalTransactionRepository),
                        _ => _personalTransactionRepositoryMock.Object
                    )
                );
            });
        });
    }

    [Fact()]
    public async Task Create_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var createCommand = new CreatePersonalTransactionCommand()
        {
            TransactionType = "Income",
            Amount = 20,
            CategoryId = 1,
            WalletId = 2,
            Timestamp = DateTime.Now,
            Note = "Test transaction",
        };
        int createdId = 42;

        _mediatorMock
            .Setup(m =>
                m.Send(It.IsAny<CreatePersonalTransactionCommand>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(createdId);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/PersonalTransactions", createCommand);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response
            .Headers.Location.ToString()
            .Should()
            .Contain($"/api/PersonalTransactions/{createdId}");
        _mediatorMock.Verify(
            m =>
                m.Send(It.IsAny<CreatePersonalTransactionCommand>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("api/PersonalTransactions");

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllPersonalTransactionsQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task GetById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 11;
        var personalTransactionDto = new PersonalTransactionDto
        {
            Id = id,
            TransactionType = "Income",
            Amount = 100,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<GetPersonalTransactionByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(personalTransactionDto);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/PersonalTransactions/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<GetPersonalTransactionByIdQuery>(q => q.Id == id),
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
        var updateCommand = new UpdatePersonalTransactionCommand
        {
            Id = id,
            TransactionType = "Income",
            Amount = 20,
            CategoryId = 1,
            WalletId = 2,
            Timestamp = DateTime.Now,
            Note = "Test transaction",
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<UpdatePersonalTransactionCommand>(c => c.Id == id),
                    It.IsAny<CancellationToken>()
                )
            )
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PatchAsync(
            $"api/PersonalTransactions/{id}",
            JsonContent.Create(updateCommand)
        );

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<UpdatePersonalTransactionCommand>(c => c.Id == id),
                    It.IsAny<CancellationToken>()
                ),
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
                m.Send(
                    It.Is<DeletePersonalTransactionCommand>(c => c.Id == id),
                    It.IsAny<CancellationToken>()
                )
            )
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"api/PersonalTransactions/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<DeletePersonalTransactionCommand>(c => c.Id == id),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact()]
    public async Task GetReportOnCategoriesTest_ReturnsOkWithReport()
    {
        // Arrange
        var query = new GetReportOnCategoriesQuery { Type = "expense", Days = 30 };

        var totalPerCategoryDtos = new List<TotalPerCategoryDto>
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

        var reportResult = new ReportOnCategoriesDto
        {
            GrandTotal = 275.75m,
            Categories = totalPerCategoryDtos,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<GetReportOnCategoriesQuery>(q =>
                        q.Type == query.Type && q.Days == query.Days
                    ),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(reportResult);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(
            $"api/PersonalTransactions/report/categories?Type={query.Type}&Days={query.Days}"
        );

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseContent = await response.Content.ReadFromJsonAsync<ReportOnCategoriesDto>();
        responseContent.Should().NotBeNull();
        responseContent.GrandTotal.Should().Be(275.75m);
        responseContent.Categories.Should().HaveCount(3);

        var categories = responseContent.Categories.ToList();
        categories[0].CategoryTitle.Should().Be("Food");
        categories[0].Total.Should().Be(150.50m);
        categories[1].CategoryTitle.Should().Be("Transportation");
        categories[2].CategoryTitle.Should().Be("Entertainment");

        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<GetReportOnCategoriesQuery>(q =>
                        q.Type == query.Type && q.Days == query.Days
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
