using System.Net;
using System.Net.Http.Json;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Installments.Commands.PayInstallment;
using FinanceTracker.Application.Installments.Commands.ReceiveInstallment;
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

    [Fact]
    public async Task PayInstallment_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var loanId = 1;
        var expectedInstallmentId = 10;
        var command = new PayInstallmentCommand
        {
            WalletId = 5,
            Amount = 150.50m,
            Note = "Test payment",
            NextDueDate = new DateTime(2025, 6, 15),
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<PayInstallmentCommand>(c =>
                        c.LoanId == loanId
                        && c.WalletId == command.WalletId
                        && c.Amount == command.Amount
                        && c.Note == command.Note
                        && c.NextDueDate == command.NextDueDate
                    ),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(expectedInstallmentId);

        var client = _factory.CreateClient();
        var content = JsonContent.Create(command);

        // Act
        var response = await client.PostAsync($"/api/loans/{loanId}/installments/pay", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response
            .Headers.Location.PathAndQuery.Should()
            .Be($"/api/loans/{loanId}/installments/{expectedInstallmentId}");

        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<PayInstallmentCommand>(c =>
                        c.LoanId == loanId
                        && c.WalletId == command.WalletId
                        && c.Amount == command.Amount
                        && c.Note == command.Note
                        && c.NextDueDate == command.NextDueDate
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task ReceiveInstallment_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var loanId = 2;
        var expectedInstallmentId = 15;

        var command = new ReceiveInstallmentCommand
        {
            WalletId = 7,
            Amount = 200m,
            Note = "Test receipt",
            NextDueDate = new DateTime(2025, 7, 20),
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<ReceiveInstallmentCommand>(c =>
                        c.LoanId == loanId && c.Amount == command.Amount
                    ),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(expectedInstallmentId);

        var client = _factory.CreateClient();
        var content = JsonContent.Create(command);

        // Act
        var response = await client.PostAsync($"/api/loans/{loanId}/installments/receive", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response
            .Headers.Location.PathAndQuery.Should()
            .Be($"/api/loans/{loanId}/installments/{expectedInstallmentId}");

        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<ReceiveInstallmentCommand>(c =>
                        c.LoanId == loanId && c.Amount == command.Amount
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
