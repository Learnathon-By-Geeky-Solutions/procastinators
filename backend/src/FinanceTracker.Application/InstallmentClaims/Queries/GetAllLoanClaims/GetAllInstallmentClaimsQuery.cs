using FinanceTracker.Application.InstallmentClaims.Dtos;
using MediatR;

namespace FinanceTracker.Application.InstallmentClaims.Queries.GetAllLoanClaims;

public class GetAllInstallmentClaimsQuery : IRequest<IEnumerable<InstallmentClaimDto>> { }
