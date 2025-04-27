using FinanceTracker.Application.Loans.Dtos.LoanDTO;
using MediatR;

namespace FinanceTracker.Application.Loans.Queries.GetAllLoansAsLender;

public class GetAllLoansAsLenderQuery : IRequest<IEnumerable<LoanDto>> { }
