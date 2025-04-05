using FluentValidation.TestHelper;
using Xunit;


namespace FinanceTracker.Application.Wallets.Commands.CreateWallet.Tests;

public class CreateWalletCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var command = new CreateWalletCommand()
        {
            Name = "Hell",
            Type = "Bank",
            Currency = "BDT"
        };

        var validator = new CreateWalletCommandValidator();

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
            Type = "RU",
            Currency = "QAR"
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