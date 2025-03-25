using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Constants.Category;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.PersonalTransactions.Commands.DeletePersonalTransaction;

public class DeletePersonalTransactionCommandHandler(
    ILogger<DeletePersonalTransactionCommandHandler> logger,
    IUserContext userContext,
    IPersonalTransactionRepository transactionRepo,
    IWalletRepository walletRepository
) : IRequestHandler<DeletePersonalTransactionCommand>
{
    public async Task Handle(
        DeletePersonalTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser();
        var transaction = await transactionRepo.GetById(request.Id);

        if (transaction is null)
        {
            throw new NotFoundException(nameof(PersonalTransaction), request.Id.ToString());
        }
        if (user!.Id != transaction.UserId)
        {
            throw new ForbiddenException();
        }

        transaction.IsDeleted = true;
        var amount = transaction.Amount;
        if (transaction.TransactionType == TransactionTypes.Expense)
            amount *= -1;
        var wallet = await walletRepository.GetById(transaction.WalletId);
        wallet!.Balance -= amount;

        logger.LogInformation("{@t}", transaction);
        logger.LogInformation("{@w}", wallet);

        await transactionRepo.SaveChangeAsync();
    }
}
