using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Categories.Queries.GetAllCategories;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Application.Wallets.Queries.GetAllWallets;
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
}
