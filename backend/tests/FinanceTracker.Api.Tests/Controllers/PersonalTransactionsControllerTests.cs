using System.Net.Http.Json;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Categories.Queries.GetAllCategories;
using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;
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
    public void GetByIdTest() { }

    [Fact()]
    public void UpdateTest() { }

    [Fact()]
    public void DeleteTest() { }

    [Fact()]
    public void GetReportOnCategoriesTest() { }
}
