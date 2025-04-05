using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.PersonalTransactions.Dtos;

public class PersonalTransactionProfile : Profile
{
    public PersonalTransactionProfile()
    {
        CreateMap<CreatePersonalTransactionCommand, PersonalTransaction>();
        CreateMap<UpdatePersonalTransactionCommand, PersonalTransaction>();
        CreateMap<PersonalTransaction, PersonalTransactionDto>()
            .ForMember(
                dst => dst.Timestamp,
                opt => opt.MapFrom(src => src.Timestamp.ToUniversalTime())
            );
    }
}
