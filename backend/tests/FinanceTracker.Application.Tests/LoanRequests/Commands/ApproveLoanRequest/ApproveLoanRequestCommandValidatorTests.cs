using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest.Tests;

public class ApproveLoanRequestCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var command = new ApproveLoanRequestCommand() { LoanRequestId = 1, LenderWalletId = 2 };

        var validator = new ApproveLoanRequestCommandValidator();

        // Act
        var results = validator.TestValidate(command);

        // Assert
        results.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForEmptyCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new ApproveLoanRequestCommand() { LenderWalletId = 0 };

        var validator = new ApproveLoanRequestCommandValidator();

        // Act
        var results = validator.TestValidate(command);

        // Assert
        results.ShouldHaveValidationErrorFor(c => c.LenderWalletId);
    }
}
