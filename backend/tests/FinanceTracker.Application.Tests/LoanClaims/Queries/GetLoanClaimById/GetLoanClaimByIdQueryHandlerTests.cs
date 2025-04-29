using AutoMapper;
using FinanceTracker.Application.LoanClaims.Dtos;
using FinanceTracker.Application.LoanClaims.Queries.GetLoanClaimById;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Loans.Queries.GetLoanById.Tests;

public class GetLoanClaimByIdQueryHandlerTests
{
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<ILoanRepository> _LoanRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetLoanClaimByIdQueryHandler _handler;
    private readonly int _LoanId = 1;
    private readonly string _userId = "test";
    private readonly UserDto _user = new("test", "test@test.com");

    public GetLoanClaimByIdQueryHandlerTests()
    {
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _LoanRepositoryMock = new Mock<ILoanRepository>();
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new GetLoanClaimByIdQueryHandler(
            _userContextMock.Object,
            _LoanRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllLoansForCurrentUser()
    {
        // Arrange
        var query = new GetLoanClaimByIdQuery() { Id = _LoanId };

        var loanClaim = new LoanClaim()
        {
            Id = 1,
            LoanId = 2,
            IsClaimed = true,
            ClaimedAt = DateTime.Now,
        };

        var expectedDto = new LoanClaimDto()
        {
            Id = 1,
            IsClaimed = true,
            ClaimedAt = DateTime.Now,
        };

        _LoanRepositoryMock
            .Setup(repo => repo.GetLoanClaimByIdAsync(_LoanId, _userId))
            .ReturnsAsync(loanClaim);

        _mapperMock.Setup(m => m.Map<LoanClaimDto>(loanClaim)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);
        _mapperMock.Verify(m => m.Map<LoanClaimDto>(loanClaim), Times.Once);
    }

    [Fact]
    public async Task Handle_ForLoanNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var LoanId = 1;
        var command = new GetLoanClaimByIdQuery() { Id = LoanId };

        _LoanRepositoryMock
            .Setup(r => r.GetLoanClaimByIdAsync(LoanId, _userId))
            .ReturnsAsync((LoanClaim?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
}
