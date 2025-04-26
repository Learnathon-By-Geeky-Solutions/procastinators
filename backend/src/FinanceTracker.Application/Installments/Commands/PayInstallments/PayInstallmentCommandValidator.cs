using FluentValidation;

namespace FinanceTracker.Application.Installments.Commands.PayInstallments;

public class PayInstallmentCommandValidator : AbstractValidator<PayInstallmentCommand>
{
    public PayInstallmentCommandValidator()
    {
        RuleFor(x => x.LoanId).NotEmpty().WithMessage("LoanId must not be empty.");

        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.NextDueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("NextDueDate must be a future date.");
    }
}
