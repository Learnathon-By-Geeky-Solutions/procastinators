using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Wallets.Commands.UpdateWallet;

public class UpdateWalletCommandHandler(ILogger<UpdateWalletCommandHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IWalletRepository repo) : IRequestHandler<UpdateWalletCommand>
{
    public async Task Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
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

        mapper.Map(request, wallet);
        await repo.SaveChangesAsync();
        logger.LogInformation("Wallet: {@w}", wallet);
    }
}