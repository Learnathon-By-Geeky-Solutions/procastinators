using System.Net;
using System.Net.Http.Json;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.Loans.Commands.CreateLoanAsBorrower;
using FinanceTracker.Application.Loans.Commands.CreateLoanAsLender;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;
using FinanceTracker.Application.Loans.Queries.GetAllLoansAsLender;
using FinanceTracker.Application.Loans.Queries.GetLoanById;
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

public class LoansControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILoanRepository> _loanRepositoryMock = new();

    public LoansControllerTests(WebApplicationFactory<Program> factory)
    {
        _mediatorMock = new Mock<IMediator>();

        // Setup default responses for commonly used queries
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllLoansAsLenderQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new List<LoanDto>
                {
                    new LoanDto
                    {
                        Id = 1,
                        Amount = 1000,
                        Note = "First test loan",
                        IssuedAt = DateTime.UtcNow.AddDays(-30),
                        DueDate = DateTime.UtcNow.AddMonths(3),
                        DueAmount = 1100,
                    },
                    new LoanDto
                    {
                        Id = 2,
                        Amount = 500,
                        Note = "Second test loan",
                        IssuedAt = DateTime.UtcNow.AddDays(-15),
                        DueDate = DateTime.UtcNow.AddMonths(1),
                        DueAmount = 550,
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
                        typeof(ILoanRepository),
                        _ => _loanRepositoryMock.Object
                    )
                );
            });
        });
    }

    [Fact]
    public async Task GetLoanById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 5;
        var loanDto = new LoanDto
        {
            Id = id,
            Amount = 1000,
            Note = "Test loan details",
            IssuedAt = DateTime.UtcNow.AddDays(-10),
            DueDate = DateTime.UtcNow.AddMonths(3),
            DueAmount = 1050,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(It.Is<GetLoanByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(loanDto);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Loans/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetLoanByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()),
            Times.Once
        );

        var result = await response.Content.ReadFromJsonAsync<LoanDto>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetAllLoansAsLender_Returns200Ok()
    {
        // Arrange
        var query = new GetAllLoansAsLenderQuery();

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/Loans/lent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllLoansAsLenderQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAllLoansAsBorrower_Returns200Ok()
    {
        // Arrange
        var loans = new List<LoanDto>
        {
            new LoanDto
            {
                Id = 3,
                Amount = 2000,
                Note = "Borrowed loan",
                IssuedAt = DateTime.UtcNow.AddDays(-5),
                DueDate = DateTime.UtcNow.AddMonths(6),
                DueAmount = 2200,
            },
        };

        _mediatorMock
            .Setup(m =>
                m.Send(It.IsAny<GetAllLoansAsBorrowerQuery>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(loans);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/Loans/borrowed");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllLoansAsBorrowerQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );

        var result = await response.Content.ReadFromJsonAsync<List<LoanDto>>();
        result.Should().NotBeNull();
        result!.Count.Should().Be(1);
        result[0].Id.Should().Be(3);
    }

    [Fact]
    public async Task CreateLoanAsLender_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var expectedLoanId = 10;
        var command = new CreateLoanAsLenderCommand
        {
            Amount = 1500,
            DueDate = DateTime.UtcNow.AddMonths(6),
            Note = "Test lender loan",
            WalletId = 3,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(It.IsAny<CreateLoanAsLenderCommand>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(expectedLoanId);

        var client = _factory.CreateClient();
        var content = JsonContent.Create(command);

        // Act
        var response = await client.PostAsync("/api/Loans/lend", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.PathAndQuery.Should().Be($"/api/Loans/{expectedLoanId}");

        _mediatorMock.Verify(
            m => m.Send(It.IsAny<CreateLoanAsLenderCommand>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateLoanAsBorrower_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var expectedLoanId = 11;
        var command = new CreateLoanAsBorrowerCommand
        {
            Amount = 800,
            DueDate = DateTime.UtcNow.AddMonths(3),
            Note = "Test borrower loan",
            WalletId = 2,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(It.IsAny<CreateLoanAsBorrowerCommand>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(expectedLoanId);

        var client = _factory.CreateClient();
        var content = JsonContent.Create(command);

        // Act
        var response = await client.PostAsync("/api/Loans/borrow", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.PathAndQuery.Should().Be($"/api/Loans/{expectedLoanId}");

        _mediatorMock.Verify(
            m => m.Send(It.IsAny<CreateLoanAsBorrowerCommand>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
