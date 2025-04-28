using AutoMapper;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Loans.Queries.GetLoanById;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Loans.Queries.Tests;

public class GetLoanByIdQueryHandlerTests
{
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetLoanByIdQueryHandler _handler;
    private readonly string _userId = "test-user-id";
    private readonly int _loanId = 1;

    public GetLoanByIdQueryHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetLoanByIdQueryHandler(
            _loanRepositoryMock.Object,
            _userContextMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_AsLender_ShouldReturnLoanDto()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var query = new GetLoanByIdQuery { Id = _loanId };

        var loan = new Loan
        {
            Id = _loanId,
            IsDeleted = false,
            LenderId = _userId, // Current user is the lender
            Amount = 100,
            Note = "Test loan",
            IssuedAt = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(10),
            DueAmount = 110,
            LoanRequest = new LoanRequest { BorrowerId = "other-user-id" },
        };

        var loanDto = new LoanDto
        {
            Id = _loanId,
            LenderId = _userId,
            Amount = 100,
            Note = "Test loan",
            IssuedAt = loan.IssuedAt,
            DueDate = loan.DueDate,
            DueAmount = 110,
        };

        _loanRepositoryMock.Setup(repo => repo.GetByIdAsync(_loanId, _userId)).ReturnsAsync(loan);

        _mapperMock.Setup(m => m.Map<LoanDto>(loan)).Returns(loanDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(loanDto);
        _loanRepositoryMock.Verify(repo => repo.GetByIdAsync(_loanId, _userId), Times.Once);
        _mapperMock.Verify(m => m.Map<LoanDto>(loan), Times.Once);
    }

    [Fact]
    public async Task Handle_AsBorrower_ShouldReturnLoanDto()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var query = new GetLoanByIdQuery { Id = _loanId };

        var loan = new Loan
        {
            Id = _loanId,
            IsDeleted = false,
            LenderId = "other-user-id", // Current user is not the lender
            Amount = 100,
            Note = "Test loan",
            IssuedAt = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(10),
            DueAmount = 110,
            LoanRequest = new LoanRequest
            {
                BorrowerId = _userId, // Current user is the borrower
            },
        };

        var loanDto = new LoanDto
        {
            Id = _loanId,
            LenderId = "other-user-id",
            Amount = 100,
            Note = "Test loan",
            IssuedAt = loan.IssuedAt,
            DueDate = loan.DueDate,
            DueAmount = 110,
        };

        _loanRepositoryMock.Setup(repo => repo.GetByIdAsync(_loanId, _userId)).ReturnsAsync(loan);

        _mapperMock.Setup(m => m.Map<LoanDto>(loan)).Returns(loanDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(loanDto);
        _loanRepositoryMock.Verify(repo => repo.GetByIdAsync(_loanId, _userId), Times.Once);
        _mapperMock.Verify(m => m.Map<LoanDto>(loan), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentLoan_ShouldThrowNotFoundException()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var query = new GetLoanByIdQuery { Id = _loanId };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_loanId, _userId))
            .ReturnsAsync((Loan?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None)
        );

        _loanRepositoryMock.Verify(repo => repo.GetByIdAsync(_loanId, _userId), Times.Once);
        _mapperMock.Verify(m => m.Map<LoanDto>(It.IsAny<Loan>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithDeletedLoan_ShouldThrowNotFoundException()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var query = new GetLoanByIdQuery { Id = _loanId };

        var loan = new Loan
        {
            Id = _loanId,
            IsDeleted = true, // Loan is deleted
            LenderId = _userId,
            Amount = 100,
            Note = "Test loan",
            IssuedAt = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(10),
            DueAmount = 110,
        };

        _loanRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_loanId, _userId))
            .ReturnsAsync((Loan?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None)
        );

        _loanRepositoryMock.Verify(repo => repo.GetByIdAsync(_loanId, _userId), Times.Once);
        _mapperMock.Verify(m => m.Map<LoanDto>(It.IsAny<Loan>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithNullUser_ShouldThrowForbiddenException()
    {
        // Arrange
        _userContextMock.Setup(u => u.GetUser()).Returns((UserDto?)null);

        var query = new GetLoanByIdQuery { Id = _loanId };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(query, CancellationToken.None)
        );

        _loanRepositoryMock.Verify(
            repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Never
        );
        _mapperMock.Verify(m => m.Map<LoanDto>(It.IsAny<Loan>()), Times.Never);
    }
}
