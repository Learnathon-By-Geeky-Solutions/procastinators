using FluentValidation;

namespace FinanceTracker.Application.Extensions;

public static class DueDateValidationExtensions
{
    public static IRuleBuilderOptions<T, DateTime> MustBeInFuture<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder,
        int hoursInFuture
    )
    {
        return ruleBuilder
            .GreaterThan(DateTime.UtcNow.AddHours(hoursInFuture))
            .WithMessage($"Due date must be at least {hoursInFuture} hour(s) later from now");
    }
}
