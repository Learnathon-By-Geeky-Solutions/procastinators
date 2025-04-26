using MediatR;

namespace FinanceTracker.Application.Installments.Commands.PayInstallments;

public class PayInstallmentCommand : IRequest<int>
{
    public int LoanId { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default!;
    public DateTime NextDueDate { get; set; } = default!;
}
