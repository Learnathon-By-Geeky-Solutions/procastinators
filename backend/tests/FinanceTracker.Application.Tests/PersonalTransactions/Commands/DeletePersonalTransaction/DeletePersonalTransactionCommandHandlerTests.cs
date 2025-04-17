using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Constants.Category;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Commands.DeletePersonalTransaction.Tests;

public class DeletePersonalTransactionCommandHandlerTests
{
    private readonly Mock<ILogger<DeletePersonalTransactionCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<IPersonalTransactionRepository> _personalTransactionRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly DeletePersonalTransactionCommandHandler _handler;

    private readonly string _userId = "test-user-id";

    public DeletePersonalTransactionCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<DeletePersonalTransactionCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _personalTransactionRepositoryMock = new Mock<IPersonalTransactionRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new DeletePersonalTransactionCommandHandler(
                _loggerMock.Object,
                _userContextMock.Object,
                _personalTransactionRepositoryMock.Object,
                _walletRepositoryMock.Object
            );
    }
    [Fact()]
    public async Task Handle_WithValidRequest_ShouldDeletePersonalTransactionAsync()
    {
        // Arrange

        var personalTransactionId = 1;
        var categoryId = 2;
        var walletId = 3;
        var oldWalletId = 4;

        var command = new DeletePersonalTransactionCommand()
        {
            Id = personalTransactionId,
        };

        var wallet = new Wallet()
        {
            Id = walletId,
            Name = "test",
            Type = "Bank",
            Currency = "BDT",
            Balance = 1000,
            UserId = _userId
        };

        var oldWallet = new Wallet()
        {
            Id = oldWalletId,
            Name = "old wallet",
            Type = "Cash",
            Currency = "BDT",
            UserId = _userId,
            Balance = 2000
        };

        var category = new Category()
        {
            Id = categoryId,
            Title = "test",
            DefaultTransactionType = "Income",
            UserId = _userId
        };

        var personalTransaction = new PersonalTransaction()
        {
            Id = personalTransactionId,
            TransactionType = TransactionTypes.Expense,
            Amount = 300,
            WalletId = walletId,
            Wallet = wallet,
            CategoryId = categoryId,
            Category = category,
            UserId = _userId,
            IsDeleted = false
        };

        _walletRepositoryMock.Setup(r => r.GetById(walletId))
            .ReturnsAsync(wallet);

        _walletRepositoryMock.Setup(r => r.GetById(oldWalletId))
            .ReturnsAsync(oldWallet);

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId))
            .ReturnsAsync(category);

        _personalTransactionRepositoryMock.Setup(r => r.GetById(personalTransactionId))
            .ReturnsAsync(personalTransaction);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Assert

        personalTransaction.IsDeleted.Should().BeTrue();
        _personalTransactionRepositoryMock.Verify(r => r.SaveChangeAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentTransaction_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeletePersonalTransactionCommand()
        {
            Id = 1
        };

        _personalTransactionRepositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync((PersonalTransaction?)null);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _personalTransactionRepositoryMock.Verify(r => r.SaveChangeAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WithDeletedTransaction_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeletePersonalTransactionCommand()
        {
            Id = 1
        };

        var transaction = new PersonalTransaction()
        {
            Id = 1,
            IsDeleted = true
        };

        _personalTransactionRepositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync(transaction);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _personalTransactionRepositoryMock.Verify(r => r.SaveChangeAsync(), Times.Never);
    }
}