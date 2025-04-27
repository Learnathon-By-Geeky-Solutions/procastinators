using FinanceTracker.Application.LoanClaims.Dtos;
using MediatR;

namespace FinanceTracker.Application.LoanClaims.Queries.GetLoanClaimById;

public class GetLoanClaimByIdQuery : IRequest<LoanClaimDto>
{
    public int Id { get; set; }
}
