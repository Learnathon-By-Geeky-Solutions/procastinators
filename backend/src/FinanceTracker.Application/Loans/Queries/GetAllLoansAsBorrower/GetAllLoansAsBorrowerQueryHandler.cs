using AutoMapper;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;

public class GetAllLoansAsBorrowerQueryHandler(
    ILoanRepository loanRepo,
    IUserContext userContext,
    IMapper mapper
) : IRequestHandler<GetAllLoansAsBorrowerQuery, IEnumerable<LoanDto>>
{
    public async Task<IEnumerable<LoanDto>> Handle(
        GetAllLoansAsBorrowerQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loans = await loanRepo.GetAllAsBorrowerAsync(user.Id);

        return mapper.Map<IEnumerable<LoanDto>>(loans);
    }
}
