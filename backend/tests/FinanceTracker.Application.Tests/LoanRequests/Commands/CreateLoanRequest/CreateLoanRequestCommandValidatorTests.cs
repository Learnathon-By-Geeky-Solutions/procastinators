using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest.Tests;

public class CreateLoanRequestCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommands_ShouldNotHaveValdiationErrors()
    {
        // Arrange
        var validator = new CreateLoanRequestCommandValidator();

        var command = new CreateLoanRequestCommand()
        {
            Amount = 1000,
            DueDate = DateTime.UtcNow.AddDays(30),
            Note = "Test loan request",
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommands_ShouldHaveValidationErrors()
    {
        // Arrange
        var validator = new CreateLoanRequestCommandValidator();

        var command = new CreateLoanRequestCommand()
        {
            Amount = -1000,
            DueDate = DateTime.UtcNow.AddDays(-1),
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Amount);
        result.ShouldHaveValidationErrorFor(c => c.DueDate);
    }
}
