using FinanceTracker.Application.Wallets.Commands.TransferFund;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction.Tests;

public class CreatePersonalTransactionCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var command = new CreatePersonalTransactionCommand()
        {
            TransactionType = "Expense",
            Amount = 45,
            WalletId = 34,
            CategoryId = 24
        };

        var validator = new CreatePersonalTransactionCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveAnyValidationErrors();
    }

    [Theory()]
    [InlineData("Income")]
    [InlineData("Expense")]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrorsForTransactionTypeProperty(string transactionTypes)
    {
        // Arrange

        var validator = new CreatePersonalTransactionCommandValidator();
        var command = new CreatePersonalTransactionCommand { TransactionType = transactionTypes };

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveValidationErrorFor(c => c.TransactionType);
    }

    [Fact()]
    public void Validator_ForNegativeAmountValues_ShouldHaveValidationErrorForTypeProperty()
    {
        // Arrange

        var validator = new CreatePersonalTransactionCommandValidator();
        var command = new CreatePersonalTransactionCommand { Amount = -98372 };

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldHaveValidationErrorFor(c => c.Amount);
    }

}