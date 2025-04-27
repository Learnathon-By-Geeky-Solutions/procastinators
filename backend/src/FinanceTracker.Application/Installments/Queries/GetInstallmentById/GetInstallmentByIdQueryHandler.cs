using AutoMapper;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Installments.Queries.GetInstallmentById;

public class GetInstallmentByIdQueryHandler(
    IUserContext userContext,
    ILoanRepository loanRepo,
    IInstallmentRepository installmentRepo,
    IMapper mapper
) : IRequestHandler<GetInstallmentByIdQuery, InstallmentDto>
{
    public async Task<InstallmentDto> Handle(
        GetInstallmentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loan =
            loanRepo.GetByIdAsync(request.LoanId, user.Id)
            ?? throw new NotFoundException(nameof(Loan), request.LoanId.ToString());
        var installment =
            await installmentRepo.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Installment), request.Id.ToString());

        return mapper.Map<InstallmentDto>(installment);
    }
}
