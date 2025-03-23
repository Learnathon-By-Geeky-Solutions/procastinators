using MediatR;

namespace FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;

public class CreatePersonalTransactionCommand: IRequest<int>
{
    public string TransactionType { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; } = default!;
    public int WalletId { get; set; } = default!;
    public int CategoryId { get; set; } = default!;
}