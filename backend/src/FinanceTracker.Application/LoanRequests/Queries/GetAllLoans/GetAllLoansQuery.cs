
using FinanceTracker.Application.LoanRequests.Dtos.LoanDTO;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllLoans;

public class GetAllLoansQuery : IRequest<IEnumerable<LoanDto>>
{
    public string LenderId { get; set; }
    public GetAllLoansQuery(string lenderId)
    {
        LenderId = lenderId;
    }
}
