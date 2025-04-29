using FinanceTracker.Application.Installments.Dtos;
using MediatR;

namespace FinanceTracker.Application.Installments.Queries.GetInstallmentById;

public class GetInstallmentByIdQuery : IRequest<InstallmentDto>
{
    public int Id { get; set; }
    public int LoanId { get; set; }
}
