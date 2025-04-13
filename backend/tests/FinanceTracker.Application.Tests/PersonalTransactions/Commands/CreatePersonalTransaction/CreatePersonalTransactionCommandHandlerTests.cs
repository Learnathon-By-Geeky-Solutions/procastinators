using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceTracker.Application.Tests.PersonalTransactions.Commands.CreatePersonalTransaction
{
    public class CreatePersonalTransactionCommandHandlerTests
    {
        //[Fact()] Ignore this test temporarily
        public async Task Handle_ForValidCommand_ReturnsCreatedTransaction()
        {
            // Arrange

            var loggerMock = new Mock<ILogger<CreatePersonalTransactionCommandHandler>>();

            var userContextMock = new Mock<IUserContext>();
            var user = new UserDto("test", "test@test.com");
            userContextMock.Setup(u => u.GetUser()).Returns(user);

            var mapperMock = new Mock<IMapper>();
            var command = new CreatePersonalTransactionCommand();
            var personalTransaction = new PersonalTransaction();
            mapperMock
                .Setup(m => m.Map<CreatePersonalTransactionCommand, PersonalTransaction>(command))
                .Returns(personalTransaction);

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.Create(It.IsAny<Category>())).ReturnsAsync(1);

            var walletRepositoryMock = new Mock<IWalletRepository>();
            walletRepositoryMock.Setup(repo => repo.Create(It.IsAny<Wallet>())).ReturnsAsync(1);

            var transactionRepositoryMock = new Mock<IPersonalTransactionRepository>();
            transactionRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<PersonalTransaction>()))
                .ReturnsAsync(1);

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
            personalTransaction.UserId.Should().Be("test");
            transactionRepositoryMock.Verify(r => r.Create(personalTransaction), Times.Once);
        }
    }
}
