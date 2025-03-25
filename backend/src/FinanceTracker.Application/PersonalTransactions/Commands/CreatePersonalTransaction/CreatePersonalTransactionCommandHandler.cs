using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Constants.Category;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;

public class CreatePersonalTransactionCommandHandler(
    ILogger<CreatePersonalTransactionCommandHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    ICategoryRepository categoryRepo,
    IWalletRepository walletRepo,
    IPersonalTransactionRepository transactionRepo
) : IRequestHandler<CreatePersonalTransactionCommand, int>
{
    public async Task<int> Handle(
        CreatePersonalTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser();
        var wallet = await walletRepo.GetById(request.WalletId);
        var category = await categoryRepo.GetById(request.CategoryId);
        AuthorizeOrThrow(user, wallet, category);

        var transaction = mapper.Map<PersonalTransaction>(request);
        transaction.UserId = user!.Id;

        var amount = request.Amount;
        if (transaction.TransactionType == TransactionTypes.Expense)
            amount *= -1;
        wallet!.Balance += amount;

        logger.LogInformation("{@t}", transaction);
        logger.LogInformation("{@w}", wallet);

        return await transactionRepo.Create(transaction);
    }

    private static void AuthorizeOrThrow(UserDto? user, Wallet? wallet, Category? category)
    {
        if (user == null || wallet?.UserId != user.Id || category?.UserId != user.Id)
        {
            throw new ForbiddenException();
        }
    }
}
