using FinanceTracker.Application.Extensions;
using FluentValidation;

namespace FinanceTracker.Application.Wallets.Commands.UpdateWallet;

public class UpdateWalletCommandValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletCommandValidator()
    {
        RuleFor(w => w.Name).NotEmpty();
        RuleFor(w => w.Type).MustBeValidWalletType();
        RuleFor(w => w.Currency).MustBeValidCurrency();
    }
}