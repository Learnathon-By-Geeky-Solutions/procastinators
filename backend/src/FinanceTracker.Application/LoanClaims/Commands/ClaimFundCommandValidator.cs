using FluentValidation;

namespace FinanceTracker.Application.LoanClaims.Commands;

public class ClaimFundCommandValidator : AbstractValidator<ClaimFundCommand>
{
    public ClaimFundCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
    }
}
