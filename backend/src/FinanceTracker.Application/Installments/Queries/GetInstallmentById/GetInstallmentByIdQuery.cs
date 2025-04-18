
using FinanceTracker.Application.Installments.Dtos;
using MediatR;

namespace FinanceTracker.Application.Installments.Queries.GetInstallmentById;

public class GetInstallmentByIdQuery(int id) : IRequest<InstallmentDto>
{
    public int Id { get; set; } = id;
}
