using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.Wallets.Commands.UpdateWallet.Tests;

public class UpdateWalletCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var command = new UpdateWalletCommand()
        {
            Name = "Test",
            Type = "Bank",
            Currency = "BDT"
        };

        var validator = new UpdateWalletCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveAnyValidationErrors();
    }
}