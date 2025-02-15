using FinanceTracker.Application.Users;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommandHandler(ILogger<CreateWalletCommandHandler> logger, IUserContext userContext): IRequestHandler<CreateWalletCommand, int>
{
    public async Task<int> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        logger.LogInformation("Creating {@wallet} for {@user}", request, user);

        return 0;
    }
}
