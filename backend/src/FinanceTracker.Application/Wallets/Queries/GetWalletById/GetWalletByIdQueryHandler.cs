using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Wallets.Queries.GetWalletById;

public class GetWalletByIdQueryHandler(ILogger<GetWalletByIdQueryHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IWalletRepository repo) : IRequestHandler<GetWalletByIdQuery, WalletDto>
{
    public async Task<WalletDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        var wallet = await repo.GetById(request.Id);
        logger.LogInformation("User: {@u} \n{@r}", user, request);

        if (wallet == null || wallet.IsDeleted)
        {
            throw new NotFoundException(nameof(Wallet), request.Id.ToString());
        }
        if (wallet.UserId != user!.Id)
        {
            throw new ForbiddenException();
        }

        return mapper.Map<WalletDto>(wallet);
    }
}