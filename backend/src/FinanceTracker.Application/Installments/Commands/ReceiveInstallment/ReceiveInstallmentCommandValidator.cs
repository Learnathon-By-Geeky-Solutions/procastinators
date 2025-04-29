using FinanceTracker.Application.Extensions;
using FinanceTracker.Application.Installments.Commands.PayInstallment;
using FluentValidation;

namespace FinanceTracker.Application.Installments.Commands.ReceiveInstallment;

public class ReceiveInstallmentCommandValidator : AbstractValidator<ReceiveInstallmentCommand>
{
    public ReceiveInstallmentCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();

        RuleFor(x => x.Amount).GreaterThan(0);

        RuleFor(x => x.NextDueDate).MustBeInFuture(1);
    }
}
