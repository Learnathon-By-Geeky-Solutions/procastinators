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
    public Task<IEnumerable<LoanRequestDto>> Handle(GetAllLoansAsBorrowerQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
