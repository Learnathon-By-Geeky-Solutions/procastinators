namespace FinanceTracker.Application.Categories.Dtos;

public class CategoryDto
{
    public int Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string DefaultTransactionType { get; set; } = default!;
}
