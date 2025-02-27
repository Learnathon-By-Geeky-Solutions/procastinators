
using MediatR;

namespace FinanceTracker.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest
{
    public int Id { get; set; }
}
