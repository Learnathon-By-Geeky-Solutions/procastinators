using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using FinanceTracker.Application.LoanRequests.Queries.GetLoanRequestById;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Queries.Tests;

public class GetLoanRequestByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetLoanRequestByIdQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<ILoanRequestRepository> _loanRequestRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetLoanRequestByIdQueryHandler _handler;
    private readonly string _userId = "test-user-id";
    private readonly int _loanRequestId = 1;

    public GetLoanRequestByIdQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetLoanRequestByIdQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _loanRequestRepositoryMock = new Mock<ILoanRequestRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetLoanRequestByIdQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _loanRequestRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequestAndBorrowerId_ShouldReturnLoanRequestDto()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var query = new GetLoanRequestByIdQuery(_loanRequestId);

        var loanRequest = new LoanRequest
        {
            Id = _loanRequestId,
            Amount = 100,
            DueDate = DateTime.UtcNow.AddDays(10),
            LenderId = "test-lender-id",
            BorrowerId = "test-borrower-id",
        };

        var loanRequestDto = new LoanRequestDto
        {
            Id = _loanRequestId,
            Amount = 100,
            DueDate = loanRequest.DueDate,
            LenderId = "test-lender-id",
            BorrowerId = "test-borrower-id",
        };

        _loanRequestRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync(loanRequest);

        _mapperMock.Setup(m => m.Map<LoanRequestDto>(loanRequest)).Returns(loanRequestDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(loanRequestDto);
        _loanRequestRepositoryMock.Verify(repo => repo.GetByIdAsync(_loanRequestId), Times.Once);
        _mapperMock.Verify(m => m.Map<LoanRequestDto>(loanRequest), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentLoanRequest_ShouldThrowNotFoundException()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var query = new GetLoanRequestByIdQuery(_loanRequestId);

        _loanRequestRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync((LoanRequest?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None)
        );

        _loanRequestRepositoryMock.Verify(repo => repo.GetByIdAsync(_loanRequestId), Times.Once);
        _mapperMock.Verify(m => m.Map<LoanRequestDto>(It.IsAny<LoanRequest>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithNullBorrowerId_ShouldThrowForbiddenException()
    {
        // Arrange
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var query = new GetLoanRequestByIdQuery(_loanRequestId);

        var loanRequest = new LoanRequest
        {
            Id = _loanRequestId,
            Amount = 100,
            DueDate = DateTime.UtcNow.AddDays(10),
            LenderId = "test-lender-id",
            BorrowerId = null!,
        };

        _loanRequestRepositoryMock
            .Setup(repo => repo.GetByIdAsync(_loanRequestId))
            .ReturnsAsync(loanRequest);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(
            () => _handler.Handle(query, CancellationToken.None)
        );

        _loanRequestRepositoryMock.Verify(repo => repo.GetByIdAsync(_loanRequestId), Times.Once);
        _mapperMock.Verify(m => m.Map<LoanRequestDto>(It.IsAny<LoanRequest>()), Times.Never);
    }
}
