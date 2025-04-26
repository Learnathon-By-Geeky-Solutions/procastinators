using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest.Tests;

public class CreateLoanRequestValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommands_ShouldNotHaveValdiationErrors()
    {
        // Arrange
        var validator = new CreateLoanRequestValidator();

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
        var validator = new CreateLoanRequestValidator();

        var command = new CreateLoanRequestCommand()
        {
            Amount = -1000,
            DueDate = DateTime.UtcNow.AddDays(-1),
            Note =
                "Test loan requestaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Amount);
        result.ShouldHaveValidationErrorFor(c => c.DueDate);
        result.ShouldHaveValidationErrorFor(c => c.Note);
    }
}
