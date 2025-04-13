using FluentValidation.TestHelper;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace FinanceTracker.Application.Wallets.Commands.TransferFund.Tests;

public class TransferFundCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var command = new TransferFundCommand()
        {
            DestinationWalletId = 2000000,
            Amount = 123432,
        };

        var validator = new TransferFundCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange

        var command = new TransferFundCommand()
        {
            SourceWalletId = 5,
            DestinationWalletId = 0,
            Amount = 0,
        };

        var validator = new TransferFundCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldHaveAnyValidationError();
    }
    [Fact()]
    public void Validator_ForNegativeAmountValues_ShouldHaveValidationErrorForTypeProperty()
    {
        // Arrange

        var validator = new TransferFundCommandValidator();
        var command = new TransferFundCommand { Amount = -98372 };

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldHaveValidationErrorFor(c => c.Amount);
    }
}