using FluentValidation;

namespace FinanceTracker.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommandValidator: AbstractValidator<CreateWalletCommand>
{
    
    public CreateWalletCommandValidator()
    {
        RuleFor(w => w.Name).NotEmpty();
        RuleFor(w => w.Type).MustBeValidWalletType();
        RuleFor(w => w.Currency).MustBeValidCurrency();
    }
}
