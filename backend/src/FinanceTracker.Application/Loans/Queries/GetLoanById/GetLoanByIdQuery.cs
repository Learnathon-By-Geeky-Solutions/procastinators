using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetLoanById;

public class GetLoanByIdQuery : IRequest<LoanDto>
{
    public int Id { get; set; }
}
