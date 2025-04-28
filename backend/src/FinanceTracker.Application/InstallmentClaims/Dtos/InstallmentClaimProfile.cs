using AutoMapper;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.InstallmentClaims.Dtos;

public class InstallmentClaimProfile : Profile
{
    public InstallmentClaimProfile()
    {
        CreateMap<InstallmentClaim, InstallmentClaimDto>();
    }
}
