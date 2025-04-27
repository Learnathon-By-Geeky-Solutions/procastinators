using AutoMapper;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Installments.Queries.GetAllInstallments;

public class GetAllInstallmentsQueryHandler(
    IUserContext userContext,
    ILoanRepository loanRepository,
    IInstallmentRepository installmentRepo,
    IMapper mapper
) : IRequestHandler<GetAllInstallmentsQuery, IEnumerable<InstallmentDto>>
{
    public async Task<IEnumerable<InstallmentDto>> Handle(
        GetAllInstallmentsQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loan =
            await loanRepository.GetByIdAsync(request.LoanId, user.Id)
            ?? throw new NotFoundException(nameof(Loan), request.LoanId.ToString());

        var installments = await installmentRepo.GetAllAsync(loan.Id);
        return mapper.Map<IEnumerable<InstallmentDto>>(installments);
    }
}
