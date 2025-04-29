using FinanceTracker.Application.InstallmentClaims.Dtos;
using MediatR;

namespace FinanceTracker.Application.InstallmentClaims.Queries.GetInstallmentClaimById;

public class GetInstallmentClaimByIdQuery : IRequest<InstallmentClaimDto>
{
    public int Id { get; set; }
}
