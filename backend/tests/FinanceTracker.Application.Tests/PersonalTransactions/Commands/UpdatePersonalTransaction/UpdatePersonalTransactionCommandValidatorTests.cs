using FinanceTracker.Domain.Entities;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction.Tests;

public class UpdatePersonalTransactionCommandValidatorTests
{
    [Theory()]
    [InlineData("Income")]
    [InlineData("Expense")]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors(string transactionType)
    {
        // Arrange

        var command = new UpdatePersonalTransactionCommand()
        {
            TransactionType = transactionType,
            Amount = 100,
            Timestamp = DateTime.Parse("2025-04-07"),
            CategoryId = 23,
            WalletId = 11
        };

        var validator = new UpdatePersonalTransactionCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveAnyValidationErrors();
    }
}