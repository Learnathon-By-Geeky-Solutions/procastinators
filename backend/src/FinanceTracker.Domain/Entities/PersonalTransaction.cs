namespace FinanceTracker.Domain.Entities;

public class PersonalTransaction
{
    public int Id { get; set; } = default!;
    public string TransactionType { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public DateTime Timestamp { get; set; } = default!;
    public string Note { get; set; } = default!;

    public bool IsDeleted { get; set; } = default!;

    public int WalletId { get; set; } = default!;
    public Wallet Wallet { get; set; } = default!;

    public int CategoryId { get; set; } = default!;
    public Category Category { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;

}
