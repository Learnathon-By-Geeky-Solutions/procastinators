using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllSentLoanRequest;

public class GetAllSentLoanRequestQuery : IRequest<IEnumerable<LoanRequestDto>>
{
}
