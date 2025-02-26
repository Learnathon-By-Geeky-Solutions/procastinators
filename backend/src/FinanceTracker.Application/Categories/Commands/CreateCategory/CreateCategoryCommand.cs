using MediatR;

namespace FinanceTracker.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<int>
{
    public string Title { get; set; } = default!;
    public string DefaultTransactionType { get; set; } = default!;
}
