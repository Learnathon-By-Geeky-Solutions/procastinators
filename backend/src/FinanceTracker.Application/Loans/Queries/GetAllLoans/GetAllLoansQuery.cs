using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoans;

public class GetAllLoansQuery : IRequest<IEnumerable<LoanDto>>
{
    //public String LenderId { get; set; } = default!;

    //public GetAllLoansQuery(string lenderId)
    //{
    //    LenderId = lenderId;
    //}
}
