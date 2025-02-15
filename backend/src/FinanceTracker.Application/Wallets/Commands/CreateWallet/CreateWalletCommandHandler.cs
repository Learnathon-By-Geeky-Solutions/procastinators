using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommandHandler(ILogger<CreateWalletCommandHandler> logger): IRequestHandler<CreateWalletCommand, int>
{
    public async Task<int> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateWalletHandler: {@wallet}", request);
        return 0;
    }
}
