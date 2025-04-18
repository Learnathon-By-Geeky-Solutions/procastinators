using FinanceTracker.Application.LoanRequests.Dtos.LoanRequest;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllLoanRequests;

public class GetAllLoanRequestQuery : IRequest<IEnumerable<LoanRequestDto>>
{
}
