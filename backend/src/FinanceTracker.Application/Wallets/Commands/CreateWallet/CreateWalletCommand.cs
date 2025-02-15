using MediatR;

namespace FinanceTracker.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommand : IRequest<int>
{
    public string Type { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Currency { get; set; } = default!;
}
