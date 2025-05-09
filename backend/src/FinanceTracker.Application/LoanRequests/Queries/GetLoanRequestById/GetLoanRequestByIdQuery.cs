﻿using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Queries.GetLoanRequestById;

public class GetLoanRequestByIdQuery(int id) : IRequest<LoanRequestDto>
{
    public int Id { get; set; } = id;
}
