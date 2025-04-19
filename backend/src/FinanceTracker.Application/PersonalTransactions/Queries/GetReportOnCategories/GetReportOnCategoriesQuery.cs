using FinanceTracker.Application.PersonalTransactions.Dtos;
using MediatR;

namespace FinanceTracker.Application.PersonalTransactions.Queries.GetReportOnCategories;

public class GetReportOnCategoriesQuery : IRequest<ReportOnCategoriesDto>
{
    public string? Type { get; set; }
    public int Days { get; set; } = 30;
}
