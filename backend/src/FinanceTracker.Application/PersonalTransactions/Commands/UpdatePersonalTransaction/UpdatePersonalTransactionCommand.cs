using MediatR;

namespace FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction;

public class UpdatePersonalTransactionCommand : IRequest
{
    public int Id { get; set; } = default!;
    public string TransactionType { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public string? Note { get; set; }
    public int CategoryId { get; set; } = default!;
    public int WalletId { get; set; } = default!;
}
