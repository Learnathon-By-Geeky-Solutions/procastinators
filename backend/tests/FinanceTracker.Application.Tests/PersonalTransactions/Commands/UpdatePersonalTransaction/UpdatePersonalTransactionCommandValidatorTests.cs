using FinanceTracker.Domain.Entities;
using FluentValidation.TestHelper;
using System.Globalization;
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
            Timestamp = DateTime.Parse("12-05-2022"),
            CategoryId = 23,
            WalletId = 11
        };

        var validator = new UpdatePersonalTransactionCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveAnyValidationErrors();
    }

    [Theory()]
    [InlineData("2025-04-25")]
    [InlineData("April 7, 2025 2:30 PM")]
    [InlineData("April 7, 2025 1:30 AM")]
    [InlineData("2025-04-07 1:30:00")]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrorsForTimestampProperty(DateTime timeStamp)
    {
        // Arrange

        var validator = new UpdatePersonalTransactionCommandValidator();
        var command = new UpdatePersonalTransactionCommand { Timestamp = timeStamp };

        // Act 

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveValidationErrorFor(c => c.Timestamp);
    }

}