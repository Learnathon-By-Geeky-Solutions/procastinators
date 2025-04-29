using MediatR;

namespace FinanceTracker.Application.InstallmentClaims.Commands.ClaimInstallmentFund;

public class ClaimInstallmentFundCommand : IRequest
{
    public int Id { get; set; } = default!;
    public int WalletId { get; set; } = default!;
}
