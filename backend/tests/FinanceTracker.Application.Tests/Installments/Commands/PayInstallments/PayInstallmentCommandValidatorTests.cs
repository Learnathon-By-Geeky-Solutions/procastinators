using FinanceTracker.Application.Installments.Commands.PayInstallment;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.Installments.Commands.PayInstallments.Tests;

public class PayInstallmentCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new PayInstallmentCommand()
        {
            WalletId = 1,
            Amount = 100,
            NextDueDate = DateTime.UtcNow.AddDays(1),
        };

        var validator = new PayInstallmentCommandValidator();

        // Act
        var results = validator.TestValidate(command);

        // Assert
        results.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new PayInstallmentCommand()
        {
            LoanId = 0,
            Amount = -100,
            NextDueDate = DateTime.UtcNow,
        };

        var validator = new PayInstallmentCommandValidator();

        // Act
        var results = validator.TestValidate(command);

        // Assert
        results.ShouldHaveValidationErrorFor(c => c.Amount);
        results.ShouldHaveValidationErrorFor(c => c.NextDueDate);
    }
}
