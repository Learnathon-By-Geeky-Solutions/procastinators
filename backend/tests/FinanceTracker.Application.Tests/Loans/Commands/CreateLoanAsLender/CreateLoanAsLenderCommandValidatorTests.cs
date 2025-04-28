using FinanceTracker.Application.Loans.Commands.CreateLoanAsLender;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.Loans.Commands.CreateLoan.Tests;

public class CreateLoanAsLenderCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateLoanAsLenderCommand()
        {
            Amount = 1000,
            WalletId = 1,
            DueDate = DateTime.UtcNow.AddDays(30),
        };

        var validator = new CreateLoanAsLenderCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new CreateLoanAsLenderCommand()
        {
            Amount = -1000,
            WalletId = 0,
            DueDate = DateTime.UtcNow.AddDays(-1),
        };

        var validator = new CreateLoanAsLenderCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Amount);
        result.ShouldHaveValidationErrorFor(c => c.WalletId);
        result.ShouldHaveValidationErrorFor(c => c.DueDate);
    }
}
