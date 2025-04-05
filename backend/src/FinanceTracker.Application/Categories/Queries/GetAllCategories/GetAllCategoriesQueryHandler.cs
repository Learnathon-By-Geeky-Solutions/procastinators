using AutoMapper;
using FinanceTracker.Application.Categories.Dtos;
using FinanceTracker.Application.Users;
using FinanceTracker.Application.Wallets.Dtos;
using FinanceTracker.Application.Wallets.Queries.GetAllWallets;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandlerr(ILogger<GetAllWalletsQueryHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    ICategoryRepository repo) : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var user = userContext.GetUser();
        logger.LogInformation("User: {@u}", user);
        var categories = await repo.GetAll(user!.Id);
        return mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}
