namespace FinanceTracker.Application.PersonalTransactions.Dtos;

public class PersonalTransactionDto
{
    public int Id { get; set; } = default!;
    public string TransactionType { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public string? Note { get; set; }

    public int WalletId { get; set; } = default!;

    public int CategoryId { get; set; } = default!;
}
