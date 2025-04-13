using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.Categories.Commands.UpdateCategory.Tests;

public class UpdateCategoryCommandValidatorTests
{
    [Theory()]
    [InlineData("Income")]
    [InlineData("Expense")]
    public void UpdateCategoryCommandValidator_ForValidCommand_ShouldNotHaveValidationErrors(string transactionType)
    {
        // Arrange

        var command = new UpdateCategoryCommand()
        {
            Title = "test",
            DefaultTransactionType = transactionType
        };

        var validator = new UpdateCategoryCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void UpdateCategoryCommandValidator_FoInvalidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var command = new UpdateCategoryCommand()
        {
            Title = "",
            DefaultTransactionType = "test"
        };

        var validator = new UpdateCategoryCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldHaveValidationErrorFor(c => c.Title);
        results.ShouldHaveValidationErrorFor(c => c.DefaultTransactionType);
    }
}