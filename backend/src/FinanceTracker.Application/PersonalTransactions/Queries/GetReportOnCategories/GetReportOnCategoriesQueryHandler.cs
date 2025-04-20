using AutoMapper;
using FinanceTracker.Application.PersonalTransactions.Dtos;
using FinanceTracker.Application.PersonalTransactions.Queries.GetAllPersonalTransactions;
using FinanceTracker.Application.Users;
using FinanceTracker.Domain.Exceptions;
using FinanceTracker.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetReportOnCategories;

public class GetReportOnCategoriesQueryHandler(
    ILogger<GetReportOnCategoriesQueryHandler> logger,
    IUserContext userContext,
    IMapper mapper,
    IPersonalTransactionRepository repo
) : IRequestHandler<GetReportOnCategoriesQuery, ReportOnCategoriesDto>
{
    public async Task<ReportOnCategoriesDto> Handle(
        GetReportOnCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetUser() ?? throw new ForbiddenException();

        logger.LogInformation("{@r}", request);

        var (categories, grandTotal) = await repo.GetReportOnCategories(
            user.Id,
            request.Days,
            request.Type
        );
        var report = new ReportOnCategoriesDto
        {
            Categories = mapper.Map<IEnumerable<TotalPerCategoryDto>>(categories),
            GrandTotal = grandTotal,
        };

        logger.LogInformation("{@r}", report);

        return report;
    }
}
