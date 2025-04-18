using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
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
        var transaction = await repo.GetById(request.Id);

        logger.LogInformation("User: {@u} \n{@r}", user, request);

        if (transaction == null || transaction.IsDeleted)
        {
            throw new NotFoundException(nameof(PersonalTransaction), request.Id.ToString());
        }
        if (transaction.UserId != user!.Id)
        {
            throw new ForbiddenException();
        }
        return mapper.Map<PersonalTransactionDto>(transaction);
    }
}
