using FinanceTracker.Application.PersonalTransactions.Dtos;
using MediatR;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetPersonalTransactionById;

public class GetPersonalTransactionByIdQuery : IRequest<PersonalTransactionDto>
{
    public int Id { get; set; }
}
