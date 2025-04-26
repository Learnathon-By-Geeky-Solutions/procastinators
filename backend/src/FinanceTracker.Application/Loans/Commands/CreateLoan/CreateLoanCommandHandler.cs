using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Loans.Commands.CreateLoan;

public class CreateLoanCommandHandler(
    ILogger<CreateLoanCommandHandler> logger,
    IUserContext userContext,
    ILoanRepository loanRepo,
    IWalletRepository walletRepo
) : IRequestHandler<CreateLoanCommand, int>
{
    public async Task<int> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        var lenderId = user!.Id;
        logger.LogInformation("WalletId: {@WalletId}", request.WalletId);
        var wallet = await walletRepo.GetById(request.WalletId);

        if (wallet == null)
            throw new NotFoundException("Wallet", lenderId.ToString());

        wallet.Balance -= request.Amount;

        var loan = new Loan
        {
            LenderId = user!.Id,
            Amount = request.Amount,
            Note = request.Note,
            DueDate = request.DueDate,
            IssuedAt = DateTime.UtcNow,
            DueAmount = request.Amount,
            IsDeleted = false,
            LoanRequestId = null,
            WalletId = wallet.Id,
        };

        logger.LogInformation("Creating Loan: {@Loan}", loan);

        return await loanRepo.CreateAsync(loan);
    }
}
