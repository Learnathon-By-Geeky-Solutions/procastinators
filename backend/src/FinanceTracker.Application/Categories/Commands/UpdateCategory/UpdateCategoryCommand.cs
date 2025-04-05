using MediatR;

namespace FinanceTracker.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string DefaultTransactionType { get; set; } = default!;
}
