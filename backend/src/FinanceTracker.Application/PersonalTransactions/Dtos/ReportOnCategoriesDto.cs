namespace FinanceTracker.Application.PersonalTransactions.Dtos;

public class TotalPerCategoryDto
{
    public int CategoryId { get; set; } = default!;
    public string CategoryTitle { get; set; } = default!;
    public decimal Total { get; set; } = default!;
}

public class ReportOnCategoriesDto
{
    public IEnumerable<TotalPerCategoryDto> Categories { get; set; } = default!;
    public decimal GrandTotal { get; set; } = default!;
}
