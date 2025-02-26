
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAll(string userId);
    Task<Category?> GetById(int id);
    Task<int> Create(Category category);

    Task<int> SaveChangesAsync();
}
