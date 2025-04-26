using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest.Tests
{
    public class ApproveLoanRequestCommandValidatorTests
    {
        [Fact()]
        public void Validator_ForValidCommand_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new ApproveLoanRequestCommand(1, 2);

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
            var command = new ApproveLoanRequestCommand(0, 0);

            var validator = new ApproveLoanRequestCommandValidator();

            // Act
            var results = validator.TestValidate(command);

            // Assert
            results.ShouldHaveValidationErrorFor(c => c.LoanRequestId);
            results.ShouldHaveValidationErrorFor(c => c.LenderWalletId);
        }
    }
}
