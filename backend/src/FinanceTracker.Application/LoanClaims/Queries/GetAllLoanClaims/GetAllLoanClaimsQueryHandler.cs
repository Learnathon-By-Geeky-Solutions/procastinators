using AutoMapper;
using FinanceTracker.Application.LoanClaims.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.LoanClaims.Queries.GetAllLoanClaims;

public class GetAllLoanClaimsQueryHandler(
    IUserContext userContext,
    ILoanRepository loanRepository,
    IMapper mapper
) : IRequestHandler<GetAllLoanClaimsQuery, IEnumerable<LoanClaimDto>>
{
    public async Task<IEnumerable<LoanClaimDto>> Handle(
        GetAllLoanClaimsQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loanClaims = await loanRepository.GetAllLoanClaimsAsync(user.Id);
        return mapper.Map<IEnumerable<LoanClaimDto>>(loanClaims);
    }
}
