using FinanceTracker.Application.LoanRequests.Dtos.LoanDTO;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Queries.GetLoanById;

public class GetLoanByIdQuery : IRequest<LoanDto>
{
    public int Id { get; set; }

    public GetLoanByIdQuery(int id)
    { 
        Id = id;
    }
}
