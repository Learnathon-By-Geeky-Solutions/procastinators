using FinanceTracker.Application.PersonalTransactions.Dtos;
using MediatR;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;

public class GetAllPersonalTransactionsQuery : IRequest<IEnumerable<PersonalTransactionDto>> { }
