using MediatR;

namespace FinanceTracker.Application.PersonalTransactions.Commands.DeletePersonalTransaction;

public class DeletePersonalTransactionCommand : IRequest
{
    public int Id { get; set; }
}
