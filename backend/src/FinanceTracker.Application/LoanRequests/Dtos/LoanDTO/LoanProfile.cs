using AutoMapper;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.LoanRequests.Dtos.LoanDTO;

public class LoanProfile : Profile
{
    public LoanProfile()
    {
        CreateMap<Loan, LoanDto>();
    }
}
