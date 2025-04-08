using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.Categories.Commands.CreateCategory.Tests;

public class CreateCategoryCommandValidatorTests
{
    [Theory()]
    [InlineData("Income")]
    [InlineData("Expense")]
    public void CreateCategoryCommandValidator_ForValidCommand_ShouldNotHaveValidationErrors(string transactionType)
    {
        // Arrange

        var command = new CreateCategoryCommand()
        {
            Title = "test",
            DefaultTransactionType = transactionType
        };

        var validator = new CreateCategoryCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void CreateCategoryCommandValidator_FoInvalidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var command = new CreateCategoryCommand()
        {
            Title = "",
            DefaultTransactionType = "test"
        };

        var validator = new CreateCategoryCommandValidator();

        // Act

        var results = validator.TestValidate(command);

        // Assert

        results.ShouldHaveValidationErrorFor(c => c.Title);
        results.ShouldHaveValidationErrorFor(c => c.DefaultTransactionType);
    }
}