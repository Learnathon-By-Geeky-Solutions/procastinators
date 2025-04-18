using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.LoanRequests.Commands.CreateLoan;

public class CreateLoanCommandHandler(
    ILogger<CreateLoanCommandHandler> logger,
    ILoanRepository loanRepo) : IRequestHandler<CreateLoanCommand, int>
{
    public async Task<int> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = new Loan
        {
            LenderId = request.LenderId,
            Amount = request.Amount,
            Note = request.Note,
            DueDate = request.DueDate,
            IssuedAt = DateTime.UtcNow,
            DueAmount = request.Amount,
            IsDeleted = false,
            LoanRequestId = null
        };

        logger.LogInformation("Creating Loan: {@Loan}", loan);

        return await loanRepo.CreateAsync(loan);
    }
}
