using FinanceTracker.Application.Wallets.Commands.TransferFund;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction.Tests;

public class CreatePersonalTransactionCommandValidatorTests
{
    [Theory()]
    [InlineData("Income")]
    [InlineData("Expense")]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors(string transactionTypes)
    {
        // Arrange

        var command = new CreatePersonalTransactionCommand()
        {
            TransactionType = transactionTypes,
            Amount = 45,
            WalletId = 34,
            CategoryId = 24
        };

        var validator = new CreatePersonalTransactionCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert
        results.ShouldNotHaveValidationErrorFor(c => c.TransactionType);
        results.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange

        var command = new CreatePersonalTransactionCommand()
        {
            TransactionType = "",
            Amount = -2,
        };

        var validator = new CreatePersonalTransactionCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldHaveAnyValidationError();
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