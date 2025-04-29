using FinanceTracker.Application.Installments.Dtos;
using MediatR;

namespace FinanceTracker.Application.Installments.Queries.GetAllInstallments;

public class GetAllInstallmentsQuery : IRequest<IEnumerable<InstallmentDto>>
{
    public int LoanId { get; set; }
}
