using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Wallets.Queries.GetAllWallets;

public class GetAllWalletsQueryHandler(ILogger<GetAllWalletsQueryHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IWalletRepository repo) : IRequestHandler<GetAllWalletsQuery, IEnumerable<WalletDto>>
{
    public async Task<IEnumerable<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        logger.LogInformation("User: {@u}", user);
        var wallets = await repo.GetAll(user!.Id);
        return mapper.Map<IEnumerable<WalletDto>>(wallets);
    }
}