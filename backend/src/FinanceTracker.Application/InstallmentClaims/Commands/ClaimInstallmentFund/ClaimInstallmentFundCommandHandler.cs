using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.InstallmentClaims.Commands.ClaimInstallmentFund;

public class ClaimInstallmentFundCommandHandler(
    ILogger<ClaimInstallmentFundCommandHandler> logger,
    IUserContext userContext,
    IInstallmentRepository installmentRepository,
    IWalletRepository walletRepository
) : IRequestHandler<ClaimInstallmentFundCommand>
{
    public async Task Handle(
        ClaimInstallmentFundCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var installmentClaim =
            await installmentRepository.GetInstallmentClaimByIdAsync(request.Id, user.Id)
            ?? throw new NotFoundException(nameof(LoanClaim), request.Id.ToString());

        if (installmentClaim.IsClaimed)
        {
            throw new BadRequestException("Loan is already claimed");
        }

        var wallet =
            await walletRepository.GetById(request.WalletId)
            ?? throw new NotFoundException(nameof(Wallet), request.WalletId.ToString());

        if (wallet.UserId != user.Id)
        {
            throw new ForbiddenException();
        }

        logger.LogInformation("{@l}\n{w}", installmentClaim, wallet);

        installmentClaim.IsClaimed = true;
        installmentClaim.ClaimedAt = DateTime.UtcNow;
        wallet.Balance += installmentClaim.Installment.Amount;
        await installmentRepository.SaveChangesAsync();
    }
}
