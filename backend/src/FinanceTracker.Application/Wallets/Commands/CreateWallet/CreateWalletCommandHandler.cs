using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommandHandler(
    ILogger<CreateWalletCommandHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IWalletRepository repo
) : IRequestHandler<CreateWalletCommand, int>
{
    public async Task<int> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        if (user == null)
            throw new ForbiddenException();

        var wallet = mapper.Map<CreateWalletCommand, Wallet>(request);
        wallet.UserId = user.Id;
        logger.LogInformation("Wallet: {@w}", wallet);

        return await repo.Create(wallet);
    }
}
