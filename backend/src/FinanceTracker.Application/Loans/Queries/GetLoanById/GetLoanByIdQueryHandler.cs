﻿using AutoMapper;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetLoanById;

public class GetLoanByIdQueryHandler(
    ILoanRepository loanRepo,
    IUserContext userContext,
    IMapper mapper
) : IRequestHandler<GetLoanByIdQuery, LoanDto>
{
    public async Task<LoanDto> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loan =
            await loanRepo.GetByIdAsync(request.Id, user.Id)
            ?? throw new NotFoundException(nameof(Loan), request.Id.ToString());

        return mapper.Map<LoanDto>(loan);
    }
}
