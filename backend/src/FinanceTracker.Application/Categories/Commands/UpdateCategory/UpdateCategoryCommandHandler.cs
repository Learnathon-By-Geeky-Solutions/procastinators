using AutoMapper;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ILogger<UpdateCategoryCommandHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    ICategoryRepository repo
) : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        var category = await repo.GetById(request.Id);
        logger.LogInformation("User: {@u} \n{@r}", user, request);

        if (category == null || category.IsDeleted)
        {
            throw new NotFoundException(nameof(Category), request.Id.ToString());
        }
        if (category.UserId != user!.Id)
        {
            throw new ForbiddenException();
        }

        mapper.Map(request, category);
        await repo.SaveChangesAsync();
        logger.LogInformation("Category: {@w}", category);
    }
}
