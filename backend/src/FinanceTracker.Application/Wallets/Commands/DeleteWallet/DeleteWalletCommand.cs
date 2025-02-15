using MediatR;

namespace FinanceTracker.Application.Wallets.Commands.DeleteWallet;

public class DeleteWalletCommand : IRequest
{
    public int Id { get; set; }
}
