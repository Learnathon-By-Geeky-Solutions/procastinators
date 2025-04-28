using AutoMapper;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.LoanClaims.Dtos;

public class LoanClaimProfile : Profile
{
    public LoanClaimProfile()
    {
        CreateMap<LoanClaim, LoanClaimDto>();
    }
}
