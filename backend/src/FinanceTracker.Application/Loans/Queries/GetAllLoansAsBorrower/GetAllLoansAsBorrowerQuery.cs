using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;

public class GetAllLoansAsBorrowerQuery : IRequest<IEnumerable<LoanRequestDto>>
{
}
