using FluentValidation;

namespace FinanceTracker.Application.Loans.Commands.CreateLoan;

public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Loan amount must be greater than zero.");

        RuleFor(x => x.WalletId).NotEmpty().WithMessage("WalletId must not be empty.");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Due date must be in the future.");
    }
}
