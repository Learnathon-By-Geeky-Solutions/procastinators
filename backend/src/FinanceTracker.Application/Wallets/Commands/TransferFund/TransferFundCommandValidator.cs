using FluentValidation;

namespace FinanceTracker.Application.Wallets.Commands.TransferFund;

public class TransferFundCommandValidator : AbstractValidator<TransferFundCommand>
{
    public TransferFundCommandValidator()
    {
        RuleFor(x => x.DestinationWalletId).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
    }
}
