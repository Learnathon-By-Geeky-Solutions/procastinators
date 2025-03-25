using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Constants.Category;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction;

public class UpdatePersonalTransactionCommandHandler(
    ILogger<UpdatePersonalTransactionCommandHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IPersonalTransactionRepository transactionRepo,
    IWalletRepository walletRepo,
    ICategoryRepository categoryRepo
) : IRequestHandler<UpdatePersonalTransactionCommand>
{
    public async Task Handle(
        UpdatePersonalTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await transactionRepo.GetById(request.Id);
        if (transaction == null || transaction.IsDeleted)
        {
            throw new NotFoundException(nameof(PersonalTransaction), request.Id.ToString());
        }

        var category = await categoryRepo.GetById(request.CategoryId);
        var wallet = await walletRepo.GetById(request.WalletId);
        if (category == null || category.IsDeleted)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId.ToString());
        }
        else if (wallet == null || wallet.IsDeleted)
        {
            throw new NotFoundException(nameof(Wallet), request.WalletId.ToString());
        }

        var userId = transaction.UserId;
        var user = userContext.GetUser();
        if (
            user == null
            || user.Id != userId
            || wallet.UserId != userId
            || category.UserId != userId
        )
        {
            throw new ForbiddenException();
        }

        var oldWallet = await walletRepo.GetById(transaction.WalletId);
        oldWallet!.Balance = GetRollbackedBalance(
            oldWallet.Balance,
            transaction.Amount,
            transaction.TransactionType
        );
        wallet.Balance = GetUpdatedBalance(wallet.Balance, request.Amount, request.TransactionType);
        mapper.Map(request, transaction);
        logger.LogInformation(
            "Transaction:{@t}\nPrev:{@o}\nCurr:{@n}",
            transaction,
            oldWallet,
            wallet
        );

        await transactionRepo.SaveChangeAsync();
    }

    private static decimal GetRollbackedBalance(decimal balance, decimal amount, string type)
    {
        if (type == TransactionTypes.Expense)
            amount *= -1;
        balance -= amount;
        return balance;
    }

    private static decimal GetUpdatedBalance(decimal balance, decimal amount, string type)
    {
        if (type == TransactionTypes.Expense)
            amount *= -1;
        balance += amount;
        return balance;
    }
}
