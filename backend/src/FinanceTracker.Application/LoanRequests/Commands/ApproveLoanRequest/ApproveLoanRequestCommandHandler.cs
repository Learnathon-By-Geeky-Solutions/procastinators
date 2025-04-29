using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Commands.ApproveLoanRequest;

public class ApproveLoanRequestCommandHandler(
    ILogger<ApproveLoanRequestCommandHandler> logger,
    ILoanRequestRepository loanRequestRepo,
    ILoanRepository loanRepo,
    IWalletRepository walletRepo,
    IUserContext userContext
) : IRequestHandler<ApproveLoanRequestCommand, int>
{
    public async Task<int> Handle(
        ApproveLoanRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();

        var loanRequest =
            await loanRequestRepo.GetReceivedByIdAsync(request.LoanRequestId, user.Id)
            ?? throw new NotFoundException(nameof(LoanRequest), request.LoanRequestId.ToString());

        logger.LogInformation("LoanReq: {@r}", loanRequest);

        if (loanRequest.IsApproved)
            throw new BadRequestException("LoanRequest is already approved.");

        var lenderWallet =
            await walletRepo.GetById(request.LenderWalletId)
            ?? throw new NotFoundException(nameof(Wallet), request.LenderWalletId.ToString());

        if (lenderWallet.UserId != user.Id)
            throw new ForbiddenException();

        lenderWallet.Balance -= loanRequest.Amount;
        loanRequest.IsApproved = true;

        var loan = new Loan
        {
            LenderId = loanRequest.LenderId,
            BorrowerId = loanRequest.BorrowerId,
            LoanRequestId = loanRequest.Id,
            Amount = loanRequest.Amount,
            Note = loanRequest.Note,
            IssuedAt = DateTime.UtcNow,
            DueDate = loanRequest.DueDate,
            DueAmount = loanRequest.Amount,
        };

        await loanRepo.CreateWithClaimAsync(loan);
        return loan.Id;
    }
}
