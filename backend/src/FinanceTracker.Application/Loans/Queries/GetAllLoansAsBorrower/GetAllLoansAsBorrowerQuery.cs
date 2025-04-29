using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;

public class GetAllLoansAsBorrowerQuery : IRequest<IEnumerable<LoanDto>>
{
}
