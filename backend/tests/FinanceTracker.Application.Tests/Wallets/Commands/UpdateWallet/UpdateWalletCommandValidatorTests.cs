using FinanceTracker.Application.Wallets.Commands.CreateWallet;
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
    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange

        var command = new CreateWalletCommand()
        {
            Name = "",
            Type = "abc",
            Currency = "abc"
        };

        var validator = new CreateWalletCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldHaveValidationErrorFor(c => c.Name);
        results.ShouldHaveValidationErrorFor(c => c.Type);
        results.ShouldHaveValidationErrorFor(c => c.Currency);
    }

    [Theory()]
    [InlineData("Cash")]
    [InlineData("Bank")]
    [InlineData("MFS")]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrorsForTypeProperty(string types)
    {
        // Arrange

        var validator = new CreateWalletCommandValidator();
        var command = new CreateWalletCommand { Type = types };

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveValidationErrorFor(c => c.Type);
    }

    [Theory()]
    [InlineData("BDT")]
    [InlineData("USD")]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrorsForCurrencyProperty(string currency)
    {
        // Arrange

        var validator = new CreateWalletCommandValidator();
        var command = new CreateWalletCommand { Currency = currency };

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveValidationErrorFor(c => c.Currency);
    }
}