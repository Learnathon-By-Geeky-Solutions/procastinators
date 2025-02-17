using FinanceTracker.Domain.Constants.Wallet;
using FluentValidation;

namespace FinanceTracker.Application.Wallets.Commands;

public static class WalletValidationExtensions
{
    private static readonly List<string> ValidTypes = WalletTypes.GetAll();
    private static readonly List<string> ValidCurrencies = Currencies.GetAll();

    public static IRuleBuilderOptions<T, string> MustBeValidWalletType<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => ValidTypes.Contains(value))
            .WithMessage($"Invalid Type. Valid types are: {string.Join(", ", ValidTypes)}");
    }

    public static IRuleBuilderOptions<T, string> MustBeValidCurrency<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => ValidCurrencies.Contains(value))
            .WithMessage($"Invalid Currency. Valid currencies are: {string.Join(", ", ValidCurrencies)}");
    }
}
