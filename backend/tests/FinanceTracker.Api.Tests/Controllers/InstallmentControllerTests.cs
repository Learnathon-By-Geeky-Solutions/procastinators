using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Application.Installments.Queries.GetAllInstallments;
using FinanceTracker.Application.Installments.Queries.GetInstallmentById;
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

public class InstallmentControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IInstallmentRepository> _installmentRepositoryMock = new();

    public InstallmentControllerTests(WebApplicationFactory<Program> factory)
    {
        _mediatorMock = new Mock<IMediator>();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllInstallmentsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<InstallmentDto>
                {
                    new InstallmentDto
                    {
                        Id = 1,
                        LoanId = 1,
                        Amount = 100,
                        Timestamp = DateTime.UtcNow,
                    },
                    new InstallmentDto
                    {
                        Id = 2,
                        LoanId = 1,
                        Amount = 100,
                        Timestamp = DateTime.UtcNow,
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
                        typeof(IInstallmentRepository),
                        _ => _installmentRepositoryMock.Object
                    )
                );
            });
        });
    }

    [Fact]
    public async Task GetInstallmentById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var loanId = 1;
        var id = 5;
        var installmentDto = new InstallmentDto
        {
            Id = id,
            LoanId = loanId,
            Amount = 100,
            Timestamp = DateTime.UtcNow,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<GetInstallmentByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(installmentDto);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/loans/{loanId}/installments/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<GetInstallmentByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAllInstallmentsByLoanId_ForExistingLoanId_Returns200Ok()
    {
        // Arrange
        var loanId = 1;
        var installments = new List<InstallmentDto>
        {
            new InstallmentDto
            {
                Id = 1,
                LoanId = loanId,
                Amount = 100,
                Timestamp = DateTime.UtcNow,
            },
            new InstallmentDto
            {
                Id = 2,
                LoanId = loanId,
                Amount = 100,
                Timestamp = DateTime.UtcNow,
            },
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<GetAllInstallmentsQuery>(q => q.LoanId == loanId),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(installments);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/loans/{loanId}/installments");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<GetAllInstallmentsQuery>(q => q.LoanId == loanId),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
