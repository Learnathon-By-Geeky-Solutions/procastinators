using FinanceTracker.Application.Loans.Commands.CreateLoanAsBorrower;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.Loans.Commands.CreateLoan.Tests;

public class CreateLoanAsBorrowerCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateLoanAsBorrowerCommand()
        {
            Amount = 1000,
            WalletId = 1,
            DueDate = DateTime.UtcNow.AddDays(30),
        };

        var validator = new CreateLoanAsBorrowerCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new CreateLoanAsBorrowerCommand()
        {
            Amount = -1000,
            WalletId = 0,
            DueDate = DateTime.UtcNow.AddDays(-1),
        };

        var validator = new CreateLoanAsBorrowerCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Amount);
        result.ShouldHaveValidationErrorFor(c => c.WalletId);
        result.ShouldHaveValidationErrorFor(c => c.DueDate);
    }
}
