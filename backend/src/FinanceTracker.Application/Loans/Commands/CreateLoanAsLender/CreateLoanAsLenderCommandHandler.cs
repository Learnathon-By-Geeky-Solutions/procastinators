using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Loans.Commands.CreateLoanAsLender;

public class CreateLoanAsLenderCommandHandler(
    ILogger<CreateLoanAsLenderCommandHandler> logger,
    IUserContext userContext,
    ILoanRepository loanRepo,
    IWalletRepository walletRepo
) : IRequestHandler<CreateLoanAsLenderCommand, int>
{
    public async Task<int> Handle(
        CreateLoanAsLenderCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();

        var wallet =
            await walletRepo.GetById(request.WalletId)
            ?? throw new NotFoundException("Wallet", request.WalletId.ToString());
        wallet.Balance -= request.Amount;

        if (wallet.UserId != user.Id)
        {
            throw new ForbiddenException();
        }

        var loan = new Loan
        {
            LenderId = user.Id,
            Amount = request.Amount,
            Note = request.Note,
            DueDate = request.DueDate,
            IssuedAt = DateTime.UtcNow,
            DueAmount = request.Amount,
            LenderWalletId = wallet.Id,
        };

        logger.LogInformation("Creating Loan: {@Loan}", loan);

        return await loanRepo.CreateAsync(loan);
    }
}
