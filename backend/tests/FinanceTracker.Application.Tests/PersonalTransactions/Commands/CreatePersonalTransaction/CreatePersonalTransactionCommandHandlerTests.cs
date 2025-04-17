using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Constants.Category;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Tests.PersonalTransactions.Commands.CreatePersonalTransaction;

public class CreatePersonalTransactionCommandHandlerTests
{
    private readonly Mock<ILogger<CreatePersonalTransactionCommandHandler>> _loggerMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<IPersonalTransactionRepository> _transactionRepositoryMock;
    private readonly CreatePersonalTransactionCommandHandler _handler;
    private readonly string _userId = "test-user-id";
    private readonly int _walletId = 1;
    private readonly int _categoryId = 2;
    private readonly UserDto _user;

    public CreatePersonalTransactionCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreatePersonalTransactionCommandHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _transactionRepositoryMock = new Mock<IPersonalTransactionRepository>();

        _user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser()).Returns(_user);

        _handler = new CreatePersonalTransactionCommandHandler(
            _loggerMock.Object,
            _userContextMock.Object,
            _mapperMock.Object,
            _categoryRepositoryMock.Object,
            _walletRepositoryMock.Object,
            _transactionRepositoryMock.Object
        );
    }

    [Theory()]
    [InlineData("Income")]
    [InlineData("Expense")]
    public async Task Handle_ForValidTransaction_ShouldCreateTransactionAndIncreaseBalance(string transactionType)
    {
        // Arrange
        var command = new CreatePersonalTransactionCommand
        {
            WalletId = _walletId,
            CategoryId = _categoryId,
            Amount = 100.0m,
            TransactionType = transactionType
        };

        var personalTransaction = new PersonalTransaction
        {
            WalletId = _walletId,
            CategoryId = _categoryId,
            Amount = 100.0m,
            TransactionType = transactionType
        };

        _mapperMock
            .Setup(m => m.Map<PersonalTransaction>(command))
            .Returns(personalTransaction);

        var category = new Category
        {
            Id = _categoryId,
            UserId = _userId
        };

        _categoryRepositoryMock
            .Setup(repo => repo.GetById(_categoryId))
            .ReturnsAsync(category);

        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId,
            Balance = 500.0m
        };

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_walletId))
            .ReturnsAsync(wallet);

        _transactionRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<PersonalTransaction>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        personalTransaction.UserId.Should().Be(_userId);
        wallet.UserId.Should().Be(_userId);
        category.UserId.Should().Be(_userId);

        _transactionRepositoryMock.Verify(r => r.Create(personalTransaction), Times.Once);
    }


    [Fact]
    public async Task Handle_WhenUserIsNull_ShouldThrowForbiddenException()
    {
        // Arrange
        _userContextMock.Setup(u => u.GetUser()).Returns((UserDto?)null);
        var command = new CreatePersonalTransactionCommand
        {
            WalletId = _walletId,
            CategoryId = _categoryId
        };

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _transactionRepositoryMock.Verify(r => r.Create(It.IsAny<PersonalTransaction>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenWalletIsNull_ShouldThrowForbiddenException()
    {
        // Arrange
        var command = new CreatePersonalTransactionCommand
        {
            WalletId = _walletId,
            CategoryId = _categoryId
        };

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_walletId))
            .ReturnsAsync((Wallet?)null);

        var category = new Category
        {
            Id = _categoryId,
            UserId = _userId
        };

        _categoryRepositoryMock
            .Setup(repo => repo.GetById(_categoryId))
            .ReturnsAsync(category);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _transactionRepositoryMock.Verify(r => r.Create(It.IsAny<PersonalTransaction>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenWalletBelongsToAnotherUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var command = new CreatePersonalTransactionCommand
        {
            WalletId = _walletId,
            CategoryId = _categoryId
        };

        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = "different-user-id" 
        };

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_walletId))
            .ReturnsAsync(wallet);

        var category = new Category
        {
            Id = _categoryId,
            UserId = _userId
        };

        _categoryRepositoryMock
            .Setup(repo => repo.GetById(_categoryId))
            .ReturnsAsync(category);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _transactionRepositoryMock.Verify(r => r.Create(It.IsAny<PersonalTransaction>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCategoryBelongsToAnotherUser_ShouldThrowForbiddenException()
    {
        // Arrange
        var command = new CreatePersonalTransactionCommand
        {
            WalletId = _walletId,
            CategoryId = _categoryId
        };

        var wallet = new Wallet
        {
            Id = _walletId,
            UserId = _userId
        };

        _walletRepositoryMock
            .Setup(repo => repo.GetById(_walletId))
            .ReturnsAsync(wallet);

        var category = new Category
        {
            Id = _categoryId,
            UserId = "different-user-id" 
        };

        _categoryRepositoryMock
            .Setup(repo => repo.GetById(_categoryId))
            .ReturnsAsync(category);

        // Act & Assert
        await Xunit.Assert.ThrowsAsync<ForbiddenException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _transactionRepositoryMock.Verify(r => r.Create(It.IsAny<PersonalTransaction>()), Times.Never);
    }
}
