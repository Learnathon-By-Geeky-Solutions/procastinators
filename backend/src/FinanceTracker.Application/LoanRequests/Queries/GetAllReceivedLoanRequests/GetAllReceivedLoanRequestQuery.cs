using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;

public class GetAllReceivedLoanRequestQuery : IRequest<IEnumerable<LoanRequestDto>>
{
}
