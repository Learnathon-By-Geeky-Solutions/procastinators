namespace FinanceTracker.Application.Wallets.Dtos;

public class WalletDto
{
    public int Id { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public decimal Balance { get; set; } = default!;
}
