using System.Net.Http.Json;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.InstallmentClaims.Commands.ClaimInstallmentFund;
using FinanceTracker.Application.InstallmentClaims.Dtos;
using FinanceTracker.Application.InstallmentClaims.Queries.GetAllInstallmentClaims;
using FinanceTracker.Application.InstallmentClaims.Queries.GetInstallmentClaimById;
using FinanceTracker.Application.Installments.Queries.GetInstallmentById;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;
using FinanceTracker.Application.PersonalTransactions.Queries.GetPersonalTransactionById;
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

public class InstallmentClaimsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPersonalTransactionRepository> _personalTransactionRepositoryMock =
        new();

    public InstallmentClaimsControllerTests(WebApplicationFactory<Program> factory)
    {
        _mediatorMock = new Mock<IMediator>();

        // Set up the mediator mock to return some test data
        _mediatorMock
            .Setup(m =>
                m.Send(It.IsAny<GetAllInstallmentClaimsQuery>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(
                new List<InstallmentClaimDto>
                {
                    new InstallmentClaimDto
                    {
                        Id = 1,
                        IsClaimed = true,
                        ClaimedAt = DateTime.UtcNow,
                    },
                    new InstallmentClaimDto
                    {
                        Id = 2,
                        IsClaimed = true,
                        ClaimedAt = DateTime.UtcNow,
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
    public async Task GetAllInstallmentClaim_ForValidRequest_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("api/InstallmentClaims");

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllInstallmentClaimsQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact()]
    public async Task GetInstallmentClaimById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 11;
        var installmentClaimDto = new InstallmentClaimDto
        {
            Id = id,
            IsClaimed = true,
            ClaimedAt = DateTime.UtcNow,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<GetInstallmentClaimByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(installmentClaimDto);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/InstallmentClaims/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<GetInstallmentClaimByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task ClaimInstallmentFund_ForValidRequest_Returns204NoContent()
    {
        // Arrange
        var id = 1;
        var command = new ClaimInstallmentFundCommand { WalletId = 123 };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<ClaimInstallmentFundCommand>(c => c.Id == id && c.WalletId == 123),
                    It.IsAny<CancellationToken>()
                )
            )
            .Returns(Task.CompletedTask);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync($"/api/InstallmentClaims/{id}/claim", command);

        // Log response content for debugging
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().BeNullOrEmpty();

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<ClaimInstallmentFundCommand>(c => c.Id == id && c.WalletId == 123),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
