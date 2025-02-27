using FinanceTracker.Application.Categories.Dtos;
using MediatR;

namespace FinanceTracker.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<CategoryDto>
{
    public int Id { get; set; }
}
