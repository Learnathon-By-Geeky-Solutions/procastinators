using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using FinanceTracker.Application.LoanRequests.Queries.GetAllSentLoanRequest;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests.Tests;

public class GetAllSentLoanRequestQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllSentLoanRequestQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoanRequestRepository> _loanRequestRepositoryMock;
    private readonly GetAllSentLoanRequestQueryHandler _handler;
    private readonly string _userId = "test-user-id";

    public GetAllSentLoanRequestQueryHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _loanRequestRepositoryMock = new Mock<ILoanRequestRepository>();
        _loggerMock = new Mock<ILogger<GetAllSentLoanRequestQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new GetAllSentLoanRequestQueryHandler(
            _loggerMock.Object,
            _loanRequestRepositoryMock.Object,
            _userContextMock.Object,
            _mapperMock.Object
        );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldReturnAllSentLoanRequestForCurrentUser()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);
        var query = new GetAllSentLoanRequestQuery();
        var loanRequests = new List<LoanRequest>
        {
            new LoanRequest
            {
                Id = 1,
                Amount = 100,
                DueDate = DateTime.UtcNow.AddDays(5),
                LenderId = "test-lender-id",
                BorrowerId = "test-borrower-id",
            },
            new LoanRequest
            {
                Id = 2,
                Amount = 200,
                DueDate = DateTime.UtcNow.AddDays(10),
                LenderId = "test-lender-id",
                BorrowerId = "test-borrower-id",
            },
        };
        var loanRequestDtos = new List<LoanRequestDto>
        {
            new LoanRequestDto
            {
                Id = 1,
                Amount = 100,
                DueDate = DateTime.UtcNow.AddDays(5),
            },
            new LoanRequestDto
            {
                Id = 2,
                Amount = 200,
                DueDate = DateTime.UtcNow.AddDays(10),
            },
        };

        _loanRequestRepositoryMock
            .Setup(repo => repo.GetAllSentAsync(_userId))
            .ReturnsAsync(loanRequests);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<LoanRequestDto>>(It.IsAny<IEnumerable<LoanRequest>>()))
            .Returns(loanRequestDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(loanRequestDtos);
        _loanRequestRepositoryMock.Verify(repo => repo.GetAllSentAsync(_userId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<LoanRequestDto>>(loanRequests), Times.Once);
    }
}
