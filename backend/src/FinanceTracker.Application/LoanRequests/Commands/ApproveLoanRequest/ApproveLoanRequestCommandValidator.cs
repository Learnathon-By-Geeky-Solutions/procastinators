using FluentValidation;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;

public class ApproveLoanRequestCommandValidator : AbstractValidator<ApproveLoanRequestCommand>
{
    public ApproveLoanRequestCommandValidator()
    {
        RuleFor(x => x.LoanRequestId).NotEmpty().WithMessage("LoanRequestId must not be empty.");

        RuleFor(x => x.LenderWalletId).NotEmpty().WithMessage("LenderWalletId must not be empty.");
    }
}
