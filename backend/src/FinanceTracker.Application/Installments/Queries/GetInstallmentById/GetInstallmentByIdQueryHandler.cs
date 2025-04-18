
using AutoMapper;
using FinanceTracker.Application.Installments.Dtos;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;

namespace FinanceTracker.Application.Installments.Queries.GetInstallmentById;

public class GetInstallmentByIdQueryHandler(
    IInstallmentRepository installmentRepo,
    IMapper mapper)
    : IRequestHandler<GetInstallmentByIdQuery, InstallmentDto>
{
    public async Task<InstallmentDto> Handle(GetInstallmentByIdQuery request, CancellationToken cancellationToken)
    {
        var installment = await installmentRepo.GetByIdAsync(request.Id);
        if (installment == null)
        {
            throw new NotFoundException("Installment", request.Id.ToString());
        }

        return mapper.Map<InstallmentDto>(installment);
    }
}
