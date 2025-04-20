using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoans;

public class GetAllLoansQuery : IRequest<IEnumerable<LoanDto>>
{
}
