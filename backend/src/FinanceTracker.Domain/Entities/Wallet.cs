namespace FinanceTracker.Domain.Entities;

public class Wallet
{
    public int Id { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public decimal Balance { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
}
