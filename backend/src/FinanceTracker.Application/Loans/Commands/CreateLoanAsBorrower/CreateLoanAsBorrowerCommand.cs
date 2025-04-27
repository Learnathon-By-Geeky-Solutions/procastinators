using MediatR;

namespace FinanceTracker.Application.Loans.Commands.CreateLoanAsBorrower;

public class CreateLoanAsBorrowerCommand : IRequest<int>
{
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default;
    public DateTime DueDate { get; set; } = default!;
    public int WalletId { get; set; } = default;
}
