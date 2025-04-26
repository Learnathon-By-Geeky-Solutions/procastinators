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
    IWalletRepository walletRepo
) : IRequestHandler<ApproveLoanRequestCommand, int>
{
    public async Task<int> Handle(
        ApproveLoanRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var loanRequest = await loanRequestRepo.GetByIdAsync(request.LoanRequestId);

        logger.LogInformation("LoanReq: {@r}", loanRequest);
        if (loanRequest == null)
            throw new NotFoundException("LoanRequest", request.LoanRequestId.ToString());

        if (loanRequest.IsApproved)
            throw new BadRequestException("LoanRequest is already approved.");

        var lenderWallet = await walletRepo.GetById(request.LenderWalletId);
        var borrowerWallet = await walletRepo.GetById(
            loanRequest.WalletId ?? throw new BadRequestException("WalletId cannot be null.")
        );

        if (lenderWallet == null)
            throw new NotFoundException("Wallet", loanRequest.LenderId.ToString());

        if (borrowerWallet == null)
            throw new NotFoundException("Wallet", loanRequest.BorrowerId.ToString());

        lenderWallet.Balance -= loanRequest.Amount;
        borrowerWallet!.Balance += loanRequest.Amount;

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
            IsDeleted = false,
            LenderWalletId = request.LenderWalletId,
            BorrowerWalletId = borrowerWallet.Id,
        };

        await loanRepo.CreateAsync(loan);
        await loanRequestRepo.SaveChangesAsync();
        return loan.Id;
    }
}
