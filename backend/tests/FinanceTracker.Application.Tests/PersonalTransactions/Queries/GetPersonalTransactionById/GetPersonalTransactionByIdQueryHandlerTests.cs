using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Application.Wallets.Queries.GetWalletById;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetPersonalTransactionById.Tests;

public class GetPersonalTransactionByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetPersonalTransactionByIdQueryHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPersonalTransactionRepository> _transactionRepositoryMock;

    private readonly GetPersonalTransactionByIdQueryHandler _handler;

    private readonly string _userId = "test-user-id";

    public GetPersonalTransactionByIdQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetPersonalTransactionByIdQueryHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _transactionRepositoryMock = new Mock<IPersonalTransactionRepository>();

        _handler = new GetPersonalTransactionByIdQueryHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _transactionRepositoryMock.Object
        );
    }
    [Fact()]
    public async Task Handle_WithValidRequest_ShouldReturnValidPersonalTransactionByIdAsync()
    {
        // Arrange
        var personalTransactionId = 1;
        var command = new GetPersonalTransactionByIdQuery()
        {
            Id = personalTransactionId,
        };
        var personalTransaction = new PersonalTransaction()
        {
            Id = personalTransactionId,
            TransactionType = "Income",
            Amount = 1000,
            UserId = _userId
        };
        var expectedDto = new PersonalTransactionDto()
        {
            Id = personalTransactionId,
            TransactionType = "Income",
            Amount = 1000,
        };

        _transactionRepositoryMock.Setup(r => r.GetById(personalTransactionId))
            .ReturnsAsync(personalTransaction);
        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);
        _mapperMock.Setup(m => m.Map<PersonalTransactionDto>(personalTransaction))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedDto);
        _mapperMock.Verify(m => m.Map<PersonalTransactionDto>(personalTransaction), Times.Once);
    }
}