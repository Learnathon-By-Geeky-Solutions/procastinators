using System.Net;
using System.Net.Http.Json;
using FinanceTracker.Api.Tests;
using FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;
using FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;
using FinanceTracker.Application.LoanRequests.Queries.GetAllSentLoanRequest;
using FinanceTracker.Application.LoanRequests.Queries.GetLoanRequestById;
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

public class LoanRequestControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILoanRequestRepository> _loanRequestRepositoryMock = new();

    public LoanRequestControllerTests(WebApplicationFactory<Program> factory)
    {
        _mediatorMock = new Mock<IMediator>();

        _mediatorMock
            .Setup(m =>
                m.Send(It.IsAny<GetAllSentLoanRequestQuery>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(
                new List<LoanRequestDto>
                {
                    new LoanRequestDto
                    {
                        Id = 1,
                        Amount = 100,
                        DueDate = DateTime.UtcNow,
                        LenderId = "2",
                        BorrowerId = "3",
                        IsApproved = true,
                    },
                    new LoanRequestDto
                    {
                        Id = 2,
                        Amount = 1040,
                        DueDate = DateTime.UtcNow,
                        LenderId = "2",
                        BorrowerId = "3",
                        IsApproved = true,
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
                        typeof(ILoanRequestRepository),
                        _ => _loanRequestRepositoryMock.Object
                    )
                );
            });
        });
    }

    [Fact]
    public async Task GetLoanRequestById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 5;
        var loanRequestDto = new LoanRequestDto
        {
            Id = id,
            Amount = 1040,
            DueDate = DateTime.UtcNow,
            LenderId = "2",
            BorrowerId = "3",
            IsApproved = true,
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<GetLoanRequestByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(loanRequestDto);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/LoanRequests/{id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<GetLoanRequestByIdQuery>(q => q.Id == id),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAllSentLoanRequests_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/LoanRequests/sent");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllSentLoanRequestQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAllReceivedLoanRequests_Returns200Ok()
    {
        // Arrange
        var receivedLoanRequests = new List<LoanRequestDto>
        {
            new LoanRequestDto
            {
                Id = 1,
                Amount = 100,
                DueDate = DateTime.UtcNow,
                LenderId = "2",
                BorrowerId = "3",
                IsApproved = false,
            },
        };

        _mediatorMock
            .Setup(m =>
                m.Send(It.IsAny<GetAllReceivedLoanRequestQuery>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(receivedLoanRequests);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/LoanRequests/received");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetAllReceivedLoanRequestQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateLoanRequest_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var expectedLoanRequestId = 10;
        var command = new CreateLoanRequestCommand
        {
            Amount = 150.50m,
            LenderId = "user123",
            DueDate = new DateTime(2025, 6, 15),
            Note = "Test loan request",
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateLoanRequestCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedLoanRequestId);

        var client = _factory.CreateClient();
        var content = JsonContent.Create(command);

        // Act
        var response = await client.PostAsync("/api/LoanRequests", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response
            .Headers.Location.PathAndQuery.Should()
            .Be($"/api/LoanRequests/{expectedLoanRequestId}");

        _mediatorMock.Verify(
            m => m.Send(It.IsAny<CreateLoanRequestCommand>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task ApproveLoanRequest_WithValidCommand_ReturnsCreatedAtAction()
    {
        // Arrange
        var loanRequestId = 5;
        var expectedLoanId = 15;
        var command = new ApproveLoanRequestCommand { LoanRequestId = 1, LenderWalletId = 2 };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<ApproveLoanRequestCommand>(c => c.LoanRequestId == loanRequestId),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(expectedLoanId);

        var client = _factory.CreateClient();
        var content = JsonContent.Create(command);

        // Act
        var response = await client.PostAsync(
            $"/api/LoanRequests/{loanRequestId}/approve",
            content
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.PathAndQuery.Should().Be($"/api/Loans/{expectedLoanId}");

        _mediatorMock.Verify(
            m =>
                m.Send(
                    It.Is<ApproveLoanRequestCommand>(c => c.LoanRequestId == loanRequestId),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
