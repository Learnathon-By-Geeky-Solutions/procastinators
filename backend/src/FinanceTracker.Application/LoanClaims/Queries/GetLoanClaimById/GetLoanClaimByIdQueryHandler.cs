using AutoMapper;
using FinanceTracker.Application.LoanClaims.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.LoanClaims.Queries.GetLoanClaimById;

public class GetLoanClaimByIdQueryHandler(
    IUserContext userContext,
    ILoanRepository loanRepository,
    IMapper mapper
) : IRequestHandler<GetLoanClaimByIdQuery, LoanClaimDto>
{
    public async Task<LoanClaimDto> Handle(
        GetLoanClaimByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loanClaim =
            await loanRepository.GetLoanClaimByIdAsync(request.Id, user.Id)
            ?? throw new NotFoundException(nameof(LoanClaim), request.Id.ToString());
        return mapper.Map<LoanClaimDto>(loanClaim);
    }
}
