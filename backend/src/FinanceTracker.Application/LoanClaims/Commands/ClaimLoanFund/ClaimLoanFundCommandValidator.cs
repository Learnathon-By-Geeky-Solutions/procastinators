using FluentValidation;

namespace FinanceTracker.Application.LoanClaims.Commands.ClaimLoanFund;

public class ClaimLoanFundCommandValidator : AbstractValidator<ClaimLoanFundCommand>
{
    public ClaimLoanFundCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();
    }
}
