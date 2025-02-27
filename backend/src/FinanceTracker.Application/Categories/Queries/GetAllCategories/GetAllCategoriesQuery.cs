using FinanceTracker.Application.Categories.Dtos;
using MediatR;

namespace FinanceTracker.Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
{

}
