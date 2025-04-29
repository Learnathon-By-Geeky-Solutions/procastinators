using MediatR;

namespace FinanceTracker.Application.LoanClaims.Commands.ClaimLoanFund;

public class ClaimLoanFundCommand : IRequest
{
    public int Id { get; set; } = default!;
    public int WalletId { get; set; } = default!;
}
