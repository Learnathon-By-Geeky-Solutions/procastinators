using FinanceTracker.Application.Extensions;
using FluentValidation;

namespace FinanceTracker.Application.Installments.Commands.PayInstallment;

public class PayInstallmentCommandValidator : AbstractValidator<PayInstallmentCommand>
{
    public PayInstallmentCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();

        RuleFor(x => x.Amount).GreaterThan(0);

        RuleFor(x => x.NextDueDate).MustBeInFuture(1);
    }
}
