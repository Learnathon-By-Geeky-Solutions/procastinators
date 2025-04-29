using AutoMapper;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Loans.Dtos.LoanDTO;

public class LoanProfile : Profile
{
    public LoanProfile()
    {
        CreateMap<Loan, LoanDto>();
    }
}
