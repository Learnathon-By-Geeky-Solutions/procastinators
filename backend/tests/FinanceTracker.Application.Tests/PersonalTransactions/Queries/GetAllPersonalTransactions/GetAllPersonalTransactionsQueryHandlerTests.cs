using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions.Tests;

public class GetAllPersonalTransactionsQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllPersonalTransactionsQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPersonalTransactionRepository> _transactionRepositoryMock;

    private readonly GetAllPersonalTransactionsQueryHandler _handler;

    private readonly string _userId = "test-user-id";

    public GetAllPersonalTransactionsQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetAllPersonalTransactionsQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _transactionRepositoryMock = new Mock<IPersonalTransactionRepository>();

        _handler = new GetAllPersonalTransactionsQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _transactionRepositoryMock.Object
        );
    }
    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnAllTransactionsForCurrentUser()
    {
        // Arrange
        var query = new GetAllPersonalTransactionsQuery();
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(user);

        var personalTransactions = new List<PersonalTransaction>
        {
            new PersonalTransaction
            {
                Id = 1,
                TransactionType = "Income",
                Amount = 1000,
                UserId = _userId
            },
            new PersonalTransaction
            {
                Id = 2,
                TransactionType = "Expense",
                Amount = 500,
                UserId = _userId
            }
        };

        _transactionRepositoryMock.Setup(r => r.GetAll(_userId))
            .ReturnsAsync(personalTransactions);

        var transactionDtos = new List<PersonalTransactionDto>
        {
            new PersonalTransactionDto
            {
                Id = 1,
                TransactionType = "Income",
                Amount = 1000
            },
            new PersonalTransactionDto
            {
                Id = 2,
                TransactionType = "Expense",
                Amount = 500
            }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<PersonalTransactionDto>>(personalTransactions))
            .Returns(transactionDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(transactionDtos);
        _transactionRepositoryMock.Verify(r => r.GetAll(_userId), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<PersonalTransactionDto>>(personalTransactions), Times.Once);
    }
    [Fact]
    public async Task Handle_WithNoAuthenticatedUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var query = new GetAllPersonalTransactionsQuery();
        _userContextMock.Setup(u => u.GetUser()).Returns((UserDto?)null);

        // Act & Assert
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
        _transactionRepositoryMock.Verify(r => r.GetAll(It.IsAny<string>()), Times.Never);
        _mapperMock.Verify(m => m.Map<IEnumerable<PersonalTransactionDto>>(It.IsAny<IEnumerable<PersonalTransaction>>()), Times.Never);
    }
}