using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests.Tests;

public class GetAllReceivedLoanRequestQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllReceivedLoanRequestQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoanRequestRepository> _loanRequestRepositoryMock;
    private readonly GetAllReceivedLoanRequestQueryHandler _handler;
    private readonly string _userId = "test-user-id";
    private readonly int _loanId = 1;

    public GetAllReceivedLoanRequestQueryHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _loanRequestRepositoryMock = new Mock<ILoanRequestRepository>();
        _loggerMock = new Mock<ILogger<GetAllReceivedLoanRequestQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new GetAllReceivedLoanRequestQueryHandler(
            _loggerMock.Object,
            _loanRequestRepositoryMock.Object,
            _userContextMock.Object,
            _mapperMock.Object
        );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldReturnAllReceivedLoanRequestForCurrentUser()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);
        var query = new GetAllReceivedLoanRequestQuery();
        var loanRequests = new List<LoanRequest>
        {
            new LoanRequest
            {
                Id = 1,
                Amount = 100,
                DueDate = DateTime.UtcNow.AddDays(5),
                WalletId = 1,
                LenderId = "test-lender-id",
                BorrowerId = "test-borrower-id",
            },
            new LoanRequest
            {
                Id = 2,
                Amount = 200,
                DueDate = DateTime.UtcNow.AddDays(10),
                WalletId = 2,
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
                LenderId = "test-lender-id",
                BorrowerId = "test-borrower-id",
            },
            new LoanRequestDto
            {
                Id = 2,
                Amount = 200,
                DueDate = DateTime.UtcNow.AddDays(10),
                LenderId = "test-lender-id",
                BorrowerId = "test-borrower-id",
            },
        };

        _loanRequestRepositoryMock
            .Setup(repo => repo.GetAllReceivedAsync(_userId))
            .ReturnsAsync(loanRequests);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<LoanRequestDto>>(It.IsAny<IEnumerable<LoanRequest>>()))
            .Returns(loanRequestDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(loanRequestDtos);
        _loanRequestRepositoryMock.Verify(repo => repo.GetAllReceivedAsync(_userId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<LoanRequestDto>>(loanRequests), Times.Once);
    }
}
