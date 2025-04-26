using FluentValidation;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;

public class CreateLoanRequestValidator : AbstractValidator<CreateLoanRequestCommand>
{
    public CreateLoanRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Loan request amount must be greater than zero.");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Due date must be in the future.");

        RuleFor(x => x.Note).MaximumLength(500).WithMessage("Note cannot exceed 500 characters.");
    }
}
