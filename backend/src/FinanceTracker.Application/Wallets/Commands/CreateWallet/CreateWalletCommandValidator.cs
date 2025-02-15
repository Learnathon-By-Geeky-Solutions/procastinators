using FinanceTracker.Domain.Constants.Wallet;
using FluentValidation;

namespace FinanceTracker.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommandValidator: AbstractValidator<CreateWalletCommand>
{
    private readonly List<string> validTypes = WalletTypes.GellAll();
    private readonly List<string> validCurrencies = Currencies.GetAll();
    public CreateWalletCommandValidator()
    {
        RuleFor(w => w.Name).NotEmpty();

        RuleFor(w => w.Type)
            .Custom((value, context) =>
            {
                if(!validTypes.Contains(value))
                {
                    context.AddFailure("Type",
                        "Invalid Type. Valid types are: " + string.Join(", ", validTypes));
                }
            });

        RuleFor(w => w.Currency)
            .Custom((value, context) =>
            {
                if (!validCurrencies.Contains(value))
                {
                    context.AddFailure("Currency",
                        "Invalid Currency. Valid currencies are: " + string.Join(", ", validCurrencies));
                }
            });
    }
}
