using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Wallets.Commands.TransferFund;

public class TransferFundCommandHandler(
    ILogger<TransferFundCommandHandler> logger,
    IUserContext userContext,
    IWalletRepository repo
) : IRequestHandler<TransferFundCommand>
{
    public async Task Handle(TransferFundCommand request, CancellationToken cancellationToken)
    {
        var sourceWallet = await repo.GetById(request.SourceWalletId);
        var destinationWallet = await repo.GetById(request.DestinationWalletId);
        if (sourceWallet == null)
        {
            throw new NotFoundException(nameof(Wallet), request.SourceWalletId.ToString());
        }
        else if (destinationWallet == null)
        {
            throw new NotFoundException(nameof(Wallet), request.DestinationWalletId.ToString());
        }
        var user = userContext.GetUser();

        if (user == null || sourceWallet.UserId != user.Id || destinationWallet.UserId != user.Id)
        {
            throw new ForbiddenException();
        }

        sourceWallet.Balance -= request.Amount;
        destinationWallet.Balance += request.Amount;

        logger.LogInformation("{@s} ", sourceWallet);
        logger.LogInformation("{@s} ", destinationWallet);

        await repo.SaveChangesAsync();
    }
}
