using FinanceTracker.Application.InstallmentClaims.Dtos;
using MediatR;

namespace FinanceTracker.Application.InstallmentClaims.Queries.GetAllInstallmentClaims;

public class GetAllInstallmentClaimsQuery : IRequest<IEnumerable<InstallmentClaimDto>> { }
