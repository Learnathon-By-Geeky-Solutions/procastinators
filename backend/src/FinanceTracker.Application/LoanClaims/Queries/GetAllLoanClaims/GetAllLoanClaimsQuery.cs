using FinanceTracker.Application.LoanClaims.Dtos;
using MediatR;

namespace FinanceTracker.Application.LoanClaims.Queries.GetAllLoanClaims;

public class GetAllLoanClaimsQuery : IRequest<IEnumerable<LoanClaimDto>> { }
