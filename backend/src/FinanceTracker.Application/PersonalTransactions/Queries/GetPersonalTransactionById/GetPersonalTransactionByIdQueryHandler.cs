using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetPersonalTransactionById;

public class GetPersonalTransactionByIdQueryHandler(
    ILogger<GetPersonalTransactionByIdQueryHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IPersonalTransactionRepository repo
) : IRequestHandler<GetPersonalTransactionByIdQuery, PersonalTransactionDto>
{
    public async Task<PersonalTransactionDto> Handle(
        GetPersonalTransactionByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser();
        if (user == null)
            throw new ForbiddenException();
        logger.LogInformation("User: {@user}", user);
        var transaction = await repo.GetById(request.Id);
        return mapper.Map<PersonalTransactionDto>(transaction);
    }
}
