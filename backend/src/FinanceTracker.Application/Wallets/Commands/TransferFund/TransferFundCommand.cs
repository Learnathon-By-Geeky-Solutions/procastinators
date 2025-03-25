using MediatR;

namespace FinanceTracker.Application.Wallets.Commands.TransferFund;

public class TransferFundCommand : IRequest
{
    public int SourceWalletId { get; set; }
    public int DestinationWalletId { get; set; }
    public decimal Amount { get; set; }
}
