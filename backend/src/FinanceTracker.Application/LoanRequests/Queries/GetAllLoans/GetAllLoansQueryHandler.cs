
using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanDTO;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.LoanRequests.Queries.GetAllLoans;

public class GetAllLoansQueryHandler(
    ILoanRepository loanRepo,
    IMapper mapper) : IRequestHandler<GetAllLoansQuery, IEnumerable<LoanDto>>
{
    public async Task<IEnumerable<LoanDto>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await loanRepo.GetAllAsync(request.LenderId);
        return mapper.Map<IEnumerable<LoanDto>>(loans);
    }
}