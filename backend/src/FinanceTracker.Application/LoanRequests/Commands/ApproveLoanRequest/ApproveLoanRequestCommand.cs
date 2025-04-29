using FinanceTracker.Domain.Entities;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;

public class ApproveLoanRequestCommand : IRequest<int>
{
    public int LoanRequestId { get; set; }
    public int LenderWalletId { get; set; }
}
