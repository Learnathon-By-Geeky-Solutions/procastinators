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
            await loanRequestRepo.GetByIdAsync(request.LoanRequestId)
            ?? throw new NotFoundException(nameof(LoanRequest), request.LoanRequestId.ToString());

        logger.LogInformation("LoanReq: {@r}", loanRequest);

        if (loanRequest.LenderId != user.Id)
            throw new ForbiddenException();

        if (loanRequest.IsApproved)
            throw new BadRequestException("LoanRequest is already approved.");

        var lenderWallet =
            await walletRepo.GetById(request.LenderWalletId)
            ?? throw new NotFoundException(nameof(Wallet), request.LenderWalletId.ToString());

        if (lenderWallet.UserId != user.Id)
            throw new ForbiddenException();

        var borrowerWallet = loanRequest.Wallet;

        lenderWallet.Balance -= loanRequest.Amount;
        borrowerWallet.Balance += loanRequest.Amount;
        loanRequest.IsApproved = true;

        var loan = new Loan
        {
            LenderId = loanRequest.LenderId,
            LoanRequestId = loanRequest.Id,
            Amount = loanRequest.Amount,
            Note = loanRequest.Note,
            IssuedAt = DateTime.UtcNow,
            DueDate = loanRequest.DueDate,
            DueAmount = loanRequest.Amount,
            LenderWalletId = lenderWallet.Id,
            BorrowerWalletId = borrowerWallet.Id,
        };

        await loanRepo.CreateAsync(loan);
        return loan.Id;
    }
}
