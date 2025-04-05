using FinanceTracker.Domain.Constants.Category;
using FluentValidation;

namespace FinanceTracker.Application.Extensions;

public static class TransactionValidationExtensions
{
    private static readonly List<string> ValidTypes = TransactionTypes.GetAll();

    public static IRuleBuilderOptions<T, string> MustBeValidTransactionType<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => ValidTypes.Contains(value))
            .WithMessage($"Invalid Type. Valid types are: {string.Join(", ", ValidTypes)}");
    }
}
