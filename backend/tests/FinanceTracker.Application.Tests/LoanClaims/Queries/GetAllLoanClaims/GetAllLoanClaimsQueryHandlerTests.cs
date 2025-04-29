using AutoMapper;
using FinanceTracker.Application.LoanClaims.Dtos;
using FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceTracker.Application.LoanClaims.Queries.GetAllLoanClaims.Tests;

public class GetAllLoanClaimsQueryHandlerTests
{
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<ILoanRepository> _LoanRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllLoanClaimsQueryHandler _handler;
    private readonly string _userId = "test";
    private readonly UserDto _user = new("test", "test@test.com");

    public GetAllLoanClaimsQueryHandlerTests()
    {
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _LoanRepositoryMock = new Mock<ILoanRepository>();
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new GetAllLoanClaimsQueryHandler(
            _userContextMock.Object,
            _LoanRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllLoansForCurrentUser()
    {
        // Arrange
        var query = new GetAllLoanClaimsQuery { };

        var loanClaim = new List<LoanClaim>
        {
            new LoanClaim
            {
                Id = 1,
                LoanId = 2,
                IsClaimed = true,
                ClaimedAt = DateTime.Now,
            },
            new LoanClaim
            {
                Id = 3,
                LoanId = 4,
                IsClaimed = true,
                ClaimedAt = DateTime.Now,
            },
        };

        var loanClaimDtos = new List<LoanClaimDto>
        {
            new LoanClaimDto
            {
                Id = 1,
                IsClaimed = true,
                ClaimedAt = DateTime.Now,
            },
            new LoanClaimDto
            {
                Id = 2,
                IsClaimed = true,
                ClaimedAt = DateTime.Now,
            },
        };

        _LoanRepositoryMock
            .Setup(repo => repo.GetAllLoanClaimsAsync(_userId))
            .ReturnsAsync(loanClaim);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<LoanClaimDto>>(It.IsAny<IEnumerable<LoanClaim>>()))
            .Returns(loanClaimDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(loanClaimDtos);
        _LoanRepositoryMock.Verify(repo => repo.GetAllLoanClaimsAsync(_userId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<LoanClaimDto>>(loanClaim), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNullUser_ThrowsForbiddenException()
    {
        // Arrange
        _userContextMock.Setup(x => x.GetUser()).Returns((UserDto?)null);
        var query = new GetAllLoanClaimsQuery { };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(query, CancellationToken.None)
        );
    }
}
