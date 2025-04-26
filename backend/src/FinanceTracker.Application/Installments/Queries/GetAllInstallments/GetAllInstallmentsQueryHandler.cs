
using AutoMapper;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Installments.Queries.GetAllInstallments;

public class GetAllInstallmentsQueryHandler(
    IInstallmentRepository installmentRepo,
    IMapper mapper)
    : IRequestHandler<GetAllInstallmentsQuery, IEnumerable<InstallmentDto>>
{
    public async Task<IEnumerable<InstallmentDto>> Handle(GetAllInstallmentsQuery request, CancellationToken cancellationToken)
    {
        var installments = await installmentRepo.GetAllByLoanIdAsync(request.LoanId);
        return mapper.Map<IEnumerable<InstallmentDto>>(installments);
    }
}