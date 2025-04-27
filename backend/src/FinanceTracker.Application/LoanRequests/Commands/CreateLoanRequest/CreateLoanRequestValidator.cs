using FinanceTracker.Application.Extensions;
using FluentValidation;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;

public class CreateLoanRequestValidator : AbstractValidator<CreateLoanRequestCommand>
{
    public CreateLoanRequestValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DueDate).MustBeInFuture(1);
    }
}
