﻿
using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanDTO;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Queries.GetLoanById;

public class GetLoanByIdQueryHandler(
    ILoanRepository loanRepo,
    IMapper mapper) : IRequestHandler<GetLoanByIdQuery, LoanDto>
{
    public async Task<LoanDto> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var loan = await loanRepo.GetByIdAsync(request.Id);
        if (loan == null || loan.IsDeleted)
            throw new NotFoundException("Loan",request.Id.ToString() );

        return mapper.Map<LoanDto>(loan);
    }
}
