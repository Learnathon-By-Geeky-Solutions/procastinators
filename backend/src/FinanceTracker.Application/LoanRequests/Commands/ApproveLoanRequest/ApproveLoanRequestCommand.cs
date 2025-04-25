using MediatR;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;

public class ApproveLoanRequestCommand(int loanRequestId) : IRequest<int>
{
    public int LoanRequestId { get; set; } = loanRequestId;
    public int LenderWalletId { get; set; } = default!;
}
