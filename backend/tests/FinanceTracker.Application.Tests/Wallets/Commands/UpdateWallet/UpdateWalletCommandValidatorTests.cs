﻿using FinanceTracker.Application.Wallets.Commands.CreateWallet;
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
            Currency = "BDT",
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

        var command = new UpdateWalletCommand()
        {
            Name = "",
            Type = "abc",
            Currency = "abc",
        };

        var validator = new UpdateWalletCommandValidator();

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

        var validator = new UpdateWalletCommandValidator();
        var command = new UpdateWalletCommand { Type = types };

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveValidationErrorFor(c => c.Type);
    }

    [Theory()]
    [InlineData("BDT")]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrorsForCurrencyProperty(
        string currency
    )
    {
        // Arrange

        var validator = new UpdateWalletCommandValidator();
        var command = new UpdateWalletCommand { Currency = currency };

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveValidationErrorFor(c => c.Currency);
    }
}
