using FinanceTracker.Application.Extensions;
using FluentValidation;

namespace FinanceTracker.Application.Loans.Commands.CreateLoanAsBorrower;

public class CreateLoanAsBorrowerCommandValidator : AbstractValidator<CreateLoanAsBorrowerCommand>
{
    public CreateLoanAsBorrowerCommandValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.DueDate).MustBeInFuture(1);
    }
}
