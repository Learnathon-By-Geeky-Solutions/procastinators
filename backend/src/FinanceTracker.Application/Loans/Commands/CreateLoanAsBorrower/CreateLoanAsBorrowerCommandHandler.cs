using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Loans.Commands.CreateLoanAsBorrower;

public class CreateLoanAsBorrowerCommandHandler(
    ILogger<CreateLoanAsBorrowerCommandHandler> logger,
    IUserContext userContext,
    ILoanRepository loanRepo,
    IWalletRepository walletRepo
) : IRequestHandler<CreateLoanAsBorrowerCommand, int>
{
    public async Task<int> Handle(
        CreateLoanAsBorrowerCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();

        var wallet =
            await walletRepo.GetById(request.WalletId)
            ?? throw new NotFoundException(nameof(Wallet), request.WalletId.ToString());

        if (wallet.IsDeleted)
        {
            throw new NotFoundException(nameof(Wallet), request.WalletId.ToString());
        }
        if (wallet.UserId != user.Id)
        {
            throw new ForbiddenException();
        }

        wallet.Balance += request.Amount;

        var loan = new Loan
        {
            BorrowerId = user.Id,
            Amount = request.Amount,
            Note = request.Note,
            DueDate = request.DueDate,
            IssuedAt = DateTime.UtcNow,
            DueAmount = request.Amount,
        };

        logger.LogInformation("{@Loan}", loan);

        return await loanRepo.CreateAsync(loan);
    }
}
