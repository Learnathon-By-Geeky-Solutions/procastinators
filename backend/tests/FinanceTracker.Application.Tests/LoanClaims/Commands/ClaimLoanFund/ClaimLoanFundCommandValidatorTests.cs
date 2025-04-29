using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.LoanClaims.Commands.ClaimLoanFund.Tests;

public class ClaimLoanFundCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new ClaimLoanFundCommand { Id = 1, WalletId = 2 };
        var validator = new ClaimLoanFundCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new ClaimLoanFundCommand { Id = 1, WalletId = 0 };
        var validator = new ClaimLoanFundCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.WalletId);
    }
}
