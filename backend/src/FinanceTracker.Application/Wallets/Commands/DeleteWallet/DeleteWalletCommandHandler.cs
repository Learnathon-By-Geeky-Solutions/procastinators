using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Wallets.Commands.DeleteWallet;

public class DeleteWalletCommandHandler(ILogger<DeleteWalletCommandHandler> logger,
    IUserContext userContext,
    IWalletRepository repo) : IRequestHandler<DeleteWalletCommand>
{
    public async Task Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
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

        wallet.IsDeleted = true;
        await repo.SaveChangesAsync();
    }
}