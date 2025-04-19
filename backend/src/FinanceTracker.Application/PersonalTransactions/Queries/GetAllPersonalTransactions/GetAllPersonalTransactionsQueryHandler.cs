using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;

public class GetAllPersonalTransactionsQueryHandler(
    ILogger<GetAllPersonalTransactionsQueryHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IPersonalTransactionRepository repo
) : IRequestHandler<GetAllPersonalTransactionsQuery, IEnumerable<PersonalTransactionDto>>
{
    public async Task<IEnumerable<PersonalTransactionDto>> Handle(
        GetAllPersonalTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser();
        if (user == null)
            throw new ForbiddenException();

        logger.LogInformation("User: {@u}", user);

        var transactions = await repo.GetAll(user.Id);
        return mapper.Map<IEnumerable<PersonalTransactionDto>>(transactions);
    }
}
