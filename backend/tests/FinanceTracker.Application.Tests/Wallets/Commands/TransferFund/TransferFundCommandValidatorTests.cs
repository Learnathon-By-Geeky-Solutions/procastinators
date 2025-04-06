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
            SourceWalletId = 1,
            DestinationWalletId = 2,
            Amount = 123432,
        };

        var validator = new TransferFundCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveAnyValidationErrors();
    }
}