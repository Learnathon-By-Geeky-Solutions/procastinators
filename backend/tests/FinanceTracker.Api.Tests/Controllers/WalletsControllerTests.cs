using System.Net.Http.Json;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Categories.Commands.DeleteCategory;
using FinanceTracker.Application.Categories.Commands.UpdateCategory;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Categories.Queries.GetAllCategories;
using FinanceTracker.Application.Categories.Queries.GetCategoryById;
using FinanceTracker.Application.Wallets.Commands.CreateWallet;
using FinanceTracker.Application.Wallets.Commands.DeleteWallet;
using FinanceTracker.Application.Wallets.Commands.TransferFund;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Application.Wallets.Queries.GetAllWallets;
using FinanceTracker.Application.Wallets.Queries.GetWalletById;
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

public class WalletsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock = new();

    public WalletsControllerTests(WebApplicationFactory<Program> factory)
    {
        _mediatorMock = new Mock<IMediator>();

        // Set up the mediator mock to return some test data
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllWalletsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<WalletDto>
                {
                    new WalletDto
                    {
                        Id = 1,
                        Type = "Cash",
                        Name = "test",
                    },
                    new WalletDto
                    {
                        Id = 2,
                        Type = "Bank",
                        Name = "test2",
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
                        typeof(IWalletRepository),
                        _ => _walletRepositoryMock.Object
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
        var result = await client.GetAsync("api/Wallets");

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllWalletsQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task GetById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 11;
        var walletDto = new WalletDto
        {
            Id = 1,
            Type = "Cash",
            Name = "test",
        };

        _mediatorMock
            .Setup(m =>
                m.Send(It.Is<GetWalletByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(walletDto);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Wallets/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetWalletByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task Create_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var createCommand = new CreateWalletCommand
        {
            Type = "Bank",
            Name = "test2",
            Currency = "BDT",
        };
        int createdId = 42;

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateWalletCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdId);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/Wallets", createCommand);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.ToString().Should().Contain($"/api/Wallets/{createdId}");
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<CreateWalletCommand>(), It.IsAny<CancellationToken>()),
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
                m.Send(It.Is<DeleteWalletCommand>(c => c.Id == id), It.IsAny<CancellationToken>())
            )
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"api/Wallets/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        _mediatorMock.Verify(
            m => m.Send(It.Is<DeleteWalletCommand>(c => c.Id == id), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task Update_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var id = 10;
        var updateCommand = new UpdateWalletCommand
        {
            Type = "Cash",
            Name = "test3",
            Currency = "BDT",
        };

        _mediatorMock
            .Setup(m =>
                m.Send(It.Is<UpdateWalletCommand>(c => c.Id == id), It.IsAny<CancellationToken>())
            )
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PatchAsync(
            $"api/Wallets/{id}",
            JsonContent.Create(updateCommand)
        );

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdateWalletCommand>(c => c.Id == id), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task Transfer_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var id = 10;
        var transferCommand = new TransferFundCommand
        {
            SourceWalletId = 5,
            DestinationWalletId = 6,
            Amount = 100,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<TransferFundCommand>(c => c.SourceWalletId == id),
                    It.IsAny<CancellationToken>()
                )
            )
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync($"api/Wallets/{id}/transfer", transferCommand);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<TransferFundCommand>(c => c.SourceWalletId == id),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
