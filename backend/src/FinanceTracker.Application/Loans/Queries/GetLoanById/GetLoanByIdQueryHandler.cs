using AutoMapper;
using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using FinanceTracker.Application.Users;
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
        var loan = await loanRepo.GetByIdAsync(request.Id);
        if (loan == null || loan.IsDeleted)
            throw new NotFoundException("Loan", request.Id.ToString());
        bool isBorrower = loan.BorrowerId == user.Id;
        bool isLender = loan.LenderId == user.Id;
        if (!isBorrower && !isLender)
            throw new ForbiddenException();
        return mapper.Map<LoanDto>(loan);
    }
}
