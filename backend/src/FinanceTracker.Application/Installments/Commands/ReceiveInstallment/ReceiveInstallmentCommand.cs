using FinanceTracker.Application.Installments.Commands.PayInstallment;
using MediatR;

namespace FinanceTracker.Application.Installments.Commands.ReceiveInstallment;

public class ReceiveInstallmentCommand : IRequest<int>
{
    public int LoanId { get; set; } = default!;
    public int WalletId { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default!;
    public DateTime NextDueDate { get; set; } = default!;
}
