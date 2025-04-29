using FluentValidation;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;

public class ApproveLoanRequestCommandValidator : AbstractValidator<ApproveLoanRequestCommand>
{
    public ApproveLoanRequestCommandValidator()
    {
        RuleFor(x => x.LenderWalletId).NotEmpty();
    }
}
