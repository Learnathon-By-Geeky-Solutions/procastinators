using MediatR;

namespace FinanceTracker.Application.Wallets.Commands.UpdateWallet;

public class UpdateWalletCommand: IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Currency { get; set; } = default!;
}
