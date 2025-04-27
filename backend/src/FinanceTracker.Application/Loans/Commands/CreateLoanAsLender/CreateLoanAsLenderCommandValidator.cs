using FinanceTracker.Application.Extensions;
using FluentValidation;

namespace FinanceTracker.Application.Loans.Commands.CreateLoanAsLender;

public class CreateLoanAsLenderCommandValidator : AbstractValidator<CreateLoanAsLenderCommand>
{
    public CreateLoanAsLenderCommandValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);

        RuleFor(x => x.WalletId).NotEmpty();

        RuleFor(x => x.DueDate).MustBeInFuture(1);
    }
}
