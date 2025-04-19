
using AutoMapper;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoans;

public class GetAllLoansQueryHandler(
    ILoanRepository loanRepo,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<GetAllLoansQuery, IEnumerable<LoanDto>>
{
    public async Task<IEnumerable<LoanDto>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        var loans = await loanRepo.GetAllAsync(user!.Id);
        return mapper.Map<IEnumerable<LoanDto>>(loans);
    }
}