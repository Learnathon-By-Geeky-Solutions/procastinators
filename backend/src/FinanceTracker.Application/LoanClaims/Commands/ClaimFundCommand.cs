using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanClaims.Commands;

public class ClaimFundCommand : IRequest
{
    public int Id { get; set; } = default!;
    public int WalletId { get; set; } = default!;
}

public class ClaimFundCommandHandler(
    ILogger<ClaimFundCommandHandler> logger,
    IUserContext userContext,
    ILoanRepository loanRepository,
    IWalletRepository walletRepository
) : IRequestHandler<ClaimFundCommand>
{
    public async Task Handle(ClaimFundCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loanClaim =
            await loanRepository.GetLoanClaimByIdAsync(request.Id, user.Id)
            ?? throw new NotFoundException(nameof(LoanClaim), request.Id.ToString());

        if (loanClaim.IsClaimed)
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

        logger.LogInformation("{@l}\n{w}", loanClaim, wallet);

        loanClaim.IsClaimed = true;
        loanClaim.ClaimedAt = DateTime.UtcNow;
        wallet.Balance += loanClaim.Loan.Amount;
        await loanRepository.SaveChangesAsync();
    }
}
