using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.CreateWallet;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(ILogger<CreateCategoryCommandHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    ICategoryRepository repo) : IRequestHandler<CreateCategoryCommand, int>
{
    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        if (user == null) throw new ForbiddenException();

        var category = mapper.Map<CreateCategoryCommand, Category>(request);
        category.UserId = user.Id;
        logger.LogInformation("Category: {@w}", category);

        return await repo.Create(category);
    }
}
