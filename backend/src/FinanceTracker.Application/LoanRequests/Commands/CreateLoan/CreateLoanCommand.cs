
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoan;

public class CreateLoanCommand : IRequest<int>
{
    //public string LenderId { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default;
    public DateTime DueDate { get; set; } = default!;
}
