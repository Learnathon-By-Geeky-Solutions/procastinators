using AutoMapper;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Installments.Dtos;

public class InstallmentProfile : Profile
{
    public InstallmentProfile()
    {
        CreateMap<Installment,InstallmentDto>();
    }
}
