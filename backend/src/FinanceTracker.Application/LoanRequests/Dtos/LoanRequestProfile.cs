using AutoMapper;
using FinanceTracker.Application.LoanRequests.Commands;
using FinanceTracker.Domain.Entities;
namespace FinanceTracker.Application.LoanRequests.Dtos;

public class LoanRequestProfile : Profile
{
    public LoanRequestProfile()
    {
        CreateMap<CreateLoanRequestCommand, LoanRequest>();
        CreateMap<LoanRequest,LoanRequestDto>();
    }
}
