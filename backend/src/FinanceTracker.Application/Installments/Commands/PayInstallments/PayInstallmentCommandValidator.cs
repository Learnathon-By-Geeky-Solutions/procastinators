using FinanceTracker.Application.Extensions;
using FluentValidation;

namespace FinanceTracker.Application.Installments.Commands.PayInstallments;

public class PayInstallmentCommandValidator : AbstractValidator<PayInstallmentCommand>
{
    public PayInstallmentCommandValidator()
    {
        RuleFor(x => x.LoanId).NotEmpty();

        RuleFor(x => x.Amount).GreaterThan(0);

        RuleFor(x => x.NextDueDate).MustBeInFuture(1);
    }
}
