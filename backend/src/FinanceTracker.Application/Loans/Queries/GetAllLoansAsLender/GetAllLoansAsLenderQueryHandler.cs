using AutoMapper;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoansAsLender;

public class GetAllLoansAsLenderQueryHandler(
    ILoanRepository loanRepo,
    IUserContext userContext,
    IMapper mapper
) : IRequestHandler<GetAllLoansAsLenderQuery, IEnumerable<LoanDto>>
{
    public async Task<IEnumerable<LoanDto>> Handle(
        GetAllLoansAsLenderQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loans = await loanRepo.GetAllAsLenderAsync(user.Id);
        return mapper.Map<IEnumerable<LoanDto>>(loans);
    }
}
