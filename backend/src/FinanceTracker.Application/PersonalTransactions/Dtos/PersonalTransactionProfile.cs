using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.PersonalTransactions.Dtos;

public class PersonalTransactionProfile : Profile
{
    public PersonalTransactionProfile()
    {
        CreateMap<CreatePersonalTransactionCommand, PersonalTransaction>();
    }
}
