using MediatR;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;

public class CreateLoanRequestCommand : IRequest<int>
{
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime DueDate { get; set; }
    public string LenderId { get; set; } = default!;
}
