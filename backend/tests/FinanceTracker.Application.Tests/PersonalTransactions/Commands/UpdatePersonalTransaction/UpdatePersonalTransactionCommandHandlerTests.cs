using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.UpdateWallet;
using FinanceTracker.Domain.Constants.Category;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction.Tests;

public class UpdatePersonalTransactionCommandHandlerTests
{
    private readonly Mock<ILogger<UpdatePersonalTransactionCommandHandler>> _loggerMock;
    private readonly Mock<IWalletRepository> _walletRepositoryMock;
    private readonly Mock<IPersonalTransactionRepository> _personalTransactionRespositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly UpdatePersonalTransactionCommandHandler _handler;

    private readonly string _userId = "test-user-id";

    public UpdatePersonalTransactionCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdatePersonalTransactionCommandHandler>>();
        _walletRepositoryMock = new Mock<IWalletRepository>();
        _personalTransactionRespositoryMock = new Mock<IPersonalTransactionRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new UpdatePersonalTransactionCommandHandler(
                _loggerMock.Object,
                _userContextMock.Object,
                _mapperMock.Object,
                _personalTransactionRespositoryMock.Object,
                _walletRepositoryMock.Object,
                _categoryRepositoryMock.Object
            );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateWallet()
    {
        // Arrange

        var personalTransactionId = 1;
        var categoryId = 2;
        var walletId = 3;
        var oldWalletId = 4;

        var command = new UpdatePersonalTransactionCommand()
        {
            Id = personalTransactionId,
            TransactionType = "Income",
            Amount = 500,
            CategoryId = categoryId,
            WalletId = walletId
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
            UserId = _userId
        };

        _walletRepositoryMock.Setup(r => r.GetById(walletId))
            .ReturnsAsync(wallet);

        _walletRepositoryMock.Setup(r => r.GetById(oldWalletId))
            .ReturnsAsync(oldWallet);

        _categoryRepositoryMock.Setup(r => r.GetById(categoryId))
            .ReturnsAsync(category);

        _personalTransactionRespositoryMock.Setup(r => r.GetById(personalTransactionId))
            .ReturnsAsync(personalTransaction);

        var user = new UserDto("test", "test@test.com") { Id = _userId };
        _userContextMock.Setup(u => u.GetUser())
            .Returns(user);

        // Act

        await _handler.Handle(command, CancellationToken.None);

        // Assert

        _personalTransactionRespositoryMock.Verify(r => r.SaveChangeAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, personalTransaction), Times.Once);

        //// Verify old wallet balance was updated (rollback the original transaction)
        //// Original balance: 2000, Original income: 300, After rollback: 2000 - 300 = 1700
        //Xunit.Assert.Equal(1700m, oldWallet.Balance);

        //// Verify new wallet balance was updated
        //// Original balance: 1000, New income: 500, After update: 1000 + 500 = 1500
        //Xunit.Assert.Equal(1500m, wallet.Balance);

    }
}