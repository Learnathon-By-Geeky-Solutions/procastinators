using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public record TotalPerCategory(Category Category, decimal Total);

public interface IPersonalTransactionRepository
{
    Task<int> Create(PersonalTransaction transaction);
    Task<IEnumerable<PersonalTransaction>> GetAll(string userId);
    Task<PersonalTransaction?> GetById(int id);
    Task<(IEnumerable<TotalPerCategory>, decimal)> GetReportOnCategories(
        string userId,
        int days,
        string? type
    );
    Task<int> SaveChangeAsync();
}
