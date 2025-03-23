using AutoMapper;
using FinanceTracker.Application.Users;
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
        if (user == null)
            throw new ForbiddenException();
        var wallet = await walletRepo.GetById(request.WalletId);
        if (wallet == null)
            throw new NotFoundException("Wallet", request.WalletId.ToString());
        var category = await categoryRepo.GetById(request.CategoryId);
        if (category == null)
            throw new NotFoundException("Category", request.CategoryId.ToString());
        if (wallet.UserId != user.Id || category.UserId != user.Id)
            throw new ForbiddenException();

        var transaction = mapper.Map<PersonalTransaction>(request);
        transaction.UserId = user.Id;
        logger.LogInformation("{@r}", transaction);
        return await transactionRepo.Create(transaction);
    }
}
