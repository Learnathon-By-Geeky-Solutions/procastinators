using AutoMapper;
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
        // Step 1: Get the loan request
        var loanRequest = await loanRequestRepo.GetByIdAsync(request.LoanRequestId);
        logger.LogInformation("LoanReq: {@r}", loanRequest);
        if (loanRequest == null)
            throw new NotFoundException("LoanRequest", request.LoanRequestId.ToString());

        if (loanRequest.IsApproved)
            throw new BadRequestException("LoanRequest is already approved.");

        var lenderWallets = await walletRepo.GetAll(loanRequest.LenderId);
        var borrowerWallets = await walletRepo.GetAll(loanRequest.BorrowerId);

        var lenderWallet = lenderWallets.FirstOrDefault(w => !w.IsDeleted);
        var borrowerWallet = borrowerWallets.FirstOrDefault(w => !w.IsDeleted);

        if (lenderWallet == null)
            throw new NotFoundException(
                "Wallet",
                $"Lender wallet not found for user {loanRequest.LenderId}"
            );

        if (lenderWallet == null)
            throw new NotFoundException(
                "Wallet",
                $"Lender wallet not found for user {loanRequest.LenderId}"
            );

        // Step 2: Approve it
        loanRequest.IsApproved = true;

        // Step 3: Create loan from request
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
        };

        // Step 4: Save loan and update request
        await loanRepo.CreateAsync(loan);
        await loanRequestRepo.SaveChangesAsync();
        return loan.Id;
    }
}
