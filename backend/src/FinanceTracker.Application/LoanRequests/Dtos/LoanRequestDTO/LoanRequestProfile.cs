﻿using AutoMapper;
using FinanceTracker.Application.LoanRequests.Commands.CreateLoanRequest;
using FinanceTracker.Domain.Entities;
namespace FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;

public class LoanRequestProfile : Profile
{
    public LoanRequestProfile()
    {
        CreateMap<CreateLoanRequestCommand, LoanRequest>();
        CreateMap<LoanRequest, LoanRequestDto>();
    }
}
