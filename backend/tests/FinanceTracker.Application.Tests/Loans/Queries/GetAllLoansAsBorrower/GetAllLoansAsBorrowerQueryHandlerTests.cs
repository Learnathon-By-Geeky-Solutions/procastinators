using AutoMapper;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoans.Tests;

public class GetAllLoansAsBorrowerQueryHandlerTests
{
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllLoansAsBorrowerQueryHandler _handler;
    private readonly string _userId = "test-user-id";

    public GetAllLoansAsBorrowerQueryHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllLoansAsBorrowerQueryHandler(
            _loanRepositoryMock.Object,
            _userContextMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllLoansForCurrentUser()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var query = new GetAllLoansAsBorrowerQuery();

        var loans = new List<Loan>
        {
            new Loan
            {
                Id = 1,
                IsDeleted = false,
                BorrowerId = "test-borrower-id",
                Amount = 100,
                Note = "First loan",
                IssuedAt = DateTime.UtcNow.AddDays(-5),
                DueDate = DateTime.UtcNow.AddDays(10),
                DueAmount = 110,
            },
            new Loan
            {
                Id = 2,
                IsDeleted = false,
                BorrowerId = "test-borrower-id",
                Amount = 200,
                Note = "Second loan",
                IssuedAt = DateTime.UtcNow.AddDays(-3),
                DueDate = DateTime.UtcNow.AddDays(15),
                DueAmount = 220,
            },
        };

        var loanDtos = new List<LoanDto>
        {
            new LoanDto
            {
                Id = 1,
                BorrowerId = "test-borrower-id",
                Amount = 100,
                Note = "First loan",
                IssuedAt = loans[0].IssuedAt,
                DueDate = loans[0].DueDate,
                DueAmount = 110,
            },
            new LoanDto
            {
                Id = 2,
                BorrowerId = "test-borrower-id",
                Amount = 200,
                Note = "Second loan",
                IssuedAt = loans[1].IssuedAt,
                DueDate = loans[1].DueDate,
                DueAmount = 220,
            },
        };

        _loanRepositoryMock.Setup(repo => repo.GetAllAsBorrowerAsync(_userId)).ReturnsAsync(loans);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<LoanDto>>(It.IsAny<IEnumerable<Loan>>()))
            .Returns(loanDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(loanDtos);
        _loanRepositoryMock.Verify(repo => repo.GetAllAsBorrowerAsync(_userId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<LoanDto>>(loans), Times.Once);
    }
}
