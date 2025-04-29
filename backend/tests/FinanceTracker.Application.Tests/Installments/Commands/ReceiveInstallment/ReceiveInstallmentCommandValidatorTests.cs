using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.Installments.Commands.ReceiveInstallment.Tests;

public class ReceiveInstallmentCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new ReceiveInstallmentCommand
        {
            WalletId = 2,
            Amount = 100,
            NextDueDate = DateTime.UtcNow.AddDays(30),
        };

        var validator = new ReceiveInstallmentCommandValidator();

        // Act
        var results = validator.TestValidate(command);

        // Assert
        results.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new ReceiveInstallmentCommand
        {
            WalletId = 0,
            Amount = -100,
            NextDueDate = DateTime.UtcNow.AddDays(-1),
        };

        var validator = new ReceiveInstallmentCommandValidator();

        // Act
        var results = validator.TestValidate(command);

        // Assert
        results.ShouldHaveValidationErrorFor(c => c.WalletId);
        results.ShouldHaveValidationErrorFor(c => c.Amount);
        results.ShouldHaveValidationErrorFor(c => c.NextDueDate);
    }
}
