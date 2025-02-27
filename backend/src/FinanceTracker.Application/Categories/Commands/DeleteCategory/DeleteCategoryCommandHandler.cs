using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Commands.DeleteWallet;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(ILogger<DeleteCategoryCommandHandler> logger,
    IUserContext userContext,
    ICategoryRepository repo) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        var category = await repo.GetById(request.Id);
        logger.LogInformation("User: {@u} \n{@r}", user, request);
        if (category == null || category.IsDeleted)
        {
            throw new NotFoundException(nameof(category), request.Id.ToString());
        }
        if (category.UserId != user!.Id)
        {
            throw new ForbiddenException();
        }

        category.IsDeleted = true;
        await repo.SaveChangesAsync();
    }
}
