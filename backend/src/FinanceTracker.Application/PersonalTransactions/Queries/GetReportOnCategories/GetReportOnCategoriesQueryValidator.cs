using FluentValidation;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetReportOnCategories;

public class GetReportOnCategoriesQueryValidator : AbstractValidator<GetReportOnCategoriesQuery>
{
    public GetReportOnCategoriesQueryValidator()
    {
        RuleFor(x => x.Days).InclusiveBetween(1, 365);
    }
}
