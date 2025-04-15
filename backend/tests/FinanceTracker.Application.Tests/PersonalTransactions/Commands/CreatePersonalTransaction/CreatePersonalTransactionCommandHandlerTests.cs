﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Constants.Category;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Tests.PersonalTransactions.Commands.CreatePersonalTransaction;

public class CreatePersonalTransactionCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCommand_ReturnsCreatedTransaction()
    {
        // Arrange
        var userId = "test-user-id";
        var walletId = 1;
        var categoryId = 2;

        // Set up logger mock
        var loggerMock = new Mock<ILogger<CreatePersonalTransactionCommandHandler>>();

        // Set up user context mock
        var userContextMock = new Mock<IUserContext>();
        var user = new UserDto("test", "test@test.com") { Id = userId };
        userContextMock.Setup(u => u.GetUser()).Returns(user);

        // Set up transaction command
        var command = new CreatePersonalTransactionCommand
        {
            WalletId = walletId,
            CategoryId = categoryId,
            Amount = 100.0m,
            TransactionType = TransactionTypes.Income
        };

        // Set up mapper
        var mapperMock = new Mock<IMapper>();
        var personalTransaction = new PersonalTransaction
        {
            WalletId = walletId,
            CategoryId = categoryId,
            Amount = 100.0m,
            TransactionType = TransactionTypes.Income
        };
        mapperMock
            .Setup(m => m.Map<PersonalTransaction>(command))
            .Returns(personalTransaction);

        // Set up category repository
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var category = new Category
        {
            Id = categoryId,
            UserId = userId // Important: Set the UserId to match the test user
        };
        categoryRepositoryMock
            .Setup(repo => repo.GetById(categoryId))
            .ReturnsAsync(category);

        // Set up wallet repository
        var walletRepositoryMock = new Mock<IWalletRepository>();
        var wallet = new Wallet
        {
            Id = walletId,
            UserId = userId, // Important: Set the UserId to match the test user
            Balance = 500.0m
        };
        walletRepositoryMock
            .Setup(repo => repo.GetById(walletId))
            .ReturnsAsync(wallet);

        // Set up transaction repository
        var transactionRepositoryMock = new Mock<IPersonalTransactionRepository>();
        transactionRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<PersonalTransaction>()))
            .ReturnsAsync(1);

        // Create command handler
        var commandHandler = new CreatePersonalTransactionCommandHandler(
            loggerMock.Object,
            userContextMock.Object,
            mapperMock.Object,
            categoryRepositoryMock.Object,
            walletRepositoryMock.Object,
            transactionRepositoryMock.Object
        );

        // Act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        personalTransaction.UserId.Should().Be(userId);
        transactionRepositoryMock.Verify(r => r.Create(personalTransaction), Times.Once);

        // Additional assertions for the wallet balance update
        wallet.Balance.Should().Be(600.0m); // Original 500 + 100 income
    }
}