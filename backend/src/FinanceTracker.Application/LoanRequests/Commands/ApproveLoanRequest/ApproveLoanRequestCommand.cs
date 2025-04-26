using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;

public class ApproveLoanRequestCommand(int loanRequestId, int lenderWalletId) : IRequest<int>
{
    public int LoanRequestId { get; set; } = loanRequestId;
    public int LenderWalletId { get; set; } = lenderWalletId;
}
