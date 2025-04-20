using AutoMapper;
using FinanceTracker.Application.LoanRequests.Dtos.LoanRequestDTO;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoansAsBorrower;

public class GetAllLoansAsBorrowerQueryHandler(ILoanRepository loanRepo,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<GetAllLoansAsBorrowerQuery, IEnumerable<LoanRequestDto>>
{
    public async Task<IEnumerable<LoanRequestDto>> Handle(GetAllLoansAsBorrowerQuery request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        var loans = await loanRepo.GetAllByBorrowerAsync(user!.Id);
        return mapper.Map<IEnumerable<LoanRequestDto>>(loans);
    }
}
