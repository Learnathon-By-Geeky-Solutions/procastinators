using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Installments.Commands.PayInstallments;

public class PayInstallmentCommandHandler(
    ILoanRepository loanRepo,
    IInstallmentRepository installmentRepo,
    IWalletRepository walletRepo,
    ILogger<PayInstallmentCommandHandler> logger
) : IRequestHandler<PayInstallmentCommand, int>
{
    public async Task<int> Handle(
        PayInstallmentCommand request,
        CancellationToken cancellationToken
    )
    {
        var loan = await loanRepo.GetByIdAsync(request.LoanId);
        if (loan == null || loan.IsDeleted)
            throw new NotFoundException("Loan", request.LoanId.ToString());

        var borrowerWallets = await walletRepo.GetAll(loan.LoanRequest!.BorrowerId);
        var lenderWallets = await walletRepo.GetAll(loan.LoanRequest.LenderId);

        var borrowerWallet = borrowerWallets.FirstOrDefault(w => !w.IsDeleted);
        var lenderWallet = lenderWallets.FirstOrDefault(w => !w.IsDeleted);

        if (borrowerWallet == null)
            throw new NotFoundException(
                "Wallet",
                $"Borrower wallet not found for user {loan.LoanRequest.BorrowerId}"
            );

        if (lenderWallet == null)
            throw new NotFoundException(
                "Wallet",
                $"Lender wallet not found for user {loan.LoanRequest.LenderId}"
            );

        if (borrowerWallet.Balance < request.Amount)
            throw new BadRequestException("Borrower does not have sufficient balance.");

        // Update wallet balances
        borrowerWallet.Balance -= request.Amount;
        lenderWallet.Balance += request.Amount;

        // Subtract the paid amount from the loan's due amount
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
        await loanRepo.SaveChangesAsync();
        await installmentRepo.SaveChangesAsync();

        logger.LogInformation("Installment paid: {@installment}", installment);
        return installment.Id;
    }
}
