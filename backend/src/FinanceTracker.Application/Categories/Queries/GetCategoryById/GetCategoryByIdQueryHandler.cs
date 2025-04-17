using AutoMapper;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Application.Wallets.Queries.GetWalletById;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(ILogger<GetCategoryByIdQueryHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    ICategoryRepository repo) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
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

        return mapper.Map<CategoryDto>(category);
    }
}
