using FluentValidation.TestHelper;
using Xunit;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetReportOnCategories.Tests;

public class GetReportOnCategoriesQueryValidatorTests
{
    [Theory()]
    [InlineData("Bank")]
    [InlineData("Cash")]
    [InlineData("MFS")]
    public void Validator_ForValidCommands_ShouldNotHaveValidationErrors(string types)
    {
        // Arrange

        var query = new GetReportOnCategoriesQuery()
        {
            Type = types,
            Days = 30
        };

        var validator = new GetReportOnCategoriesQueryValidator();

        // Act

        var results = validator.TestValidate(query);

        // Assert
        results.ShouldNotHaveAnyValidationErrors();
        results.ShouldNotHaveValidationErrorFor(c => c.Days);
    }
    [Fact()]
    public void Validator_ForInvalidCommands_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var query = new GetReportOnCategoriesQuery()
        {
            Type = "Bank",
            Days = 3550
        };

        var validator = new GetReportOnCategoriesQueryValidator();

        // Act

        var results = validator.TestValidate(query);

        // Assert
        results.ShouldHaveValidationErrorFor(c => c.Days);
    }
}