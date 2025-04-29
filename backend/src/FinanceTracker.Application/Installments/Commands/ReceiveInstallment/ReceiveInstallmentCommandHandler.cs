using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Installments.Commands.ReceiveInstallment;

public class ReceiveInstallmentCommandHandler(
    IUserContext userContext,
    ILoanRepository loanRepo,
    IInstallmentRepository installmentRepo,
    IWalletRepository walletRepo,
    ILogger<ReceiveInstallmentCommandHandler> logger
) : IRequestHandler<ReceiveInstallmentCommand, int>
{
    public async Task<int> Handle(
        ReceiveInstallmentCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();
        var loan =
            await loanRepo.GetByIdAsLenderAsync(request.LoanId, user.Id)
            ?? throw new NotFoundException(nameof(Loan), request.LoanId.ToString());

        if (loan.LoanRequestId != null)
            throw new ForbiddenException();

        var wallet =
            await walletRepo.GetById(request.WalletId)
            ?? throw new NotFoundException(nameof(Wallet), request.WalletId.ToString());

        if (wallet.IsDeleted)
            throw new NotFoundException(nameof(Wallet), request.WalletId.ToString());
        if (wallet.UserId != user.Id)
            throw new ForbiddenException();

        if (loan.DueAmount < request.Amount)
            throw new BadRequestException("Amount exceeds due amount");

        wallet.Balance += request.Amount;

        loan.DueAmount -= request.Amount;
        loan.DueDate = request.NextDueDate;

        var installment = new Installment
        {
            LoanId = request.LoanId,
            Amount = request.Amount,
            Note = request.Note,
            NextDueDate = request.NextDueDate,
        };

        await installmentRepo.CreateAsync(installment);

        logger.LogInformation("{@installment}", installment);
        return installment.Id;
    }
}
