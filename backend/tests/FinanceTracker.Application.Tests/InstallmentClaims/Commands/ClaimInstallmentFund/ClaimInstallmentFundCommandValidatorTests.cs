using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.InstallmentClaims.Commands.ClaimInstallmentFund.Tests;

public class ClaimInstallmentFundCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new ClaimInstallmentFundCommand() { Id = 1, WalletId = 2 };
        var validator = new ClaimInstallmentFundCommandValidator();

        // Act
        var results = validator.TestValidate(command);

        // Assert
        results.ShouldNotHaveAnyValidationErrors();
    }
}
