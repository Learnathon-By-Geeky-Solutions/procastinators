using FluentValidation;

namespace FinanceTracker.Application.InstallmentClaims.Commands.ClaimInstallmentFund;

public class ClaimInstallmentFundCommandValidator : AbstractValidator<ClaimInstallmentFundCommand>
{
    public ClaimInstallmentFundCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
    }
}
