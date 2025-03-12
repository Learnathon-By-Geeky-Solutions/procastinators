using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories;

public interface IPersonalTransactionRepository
{
    Task<int> Create(PersonalTransaction transaction);
    Task<IEnumerable<PersonalTransaction>> GetAll(string userId);
    Task<PersonalTransaction?> GetById(int id);
    Task<bool> Delete(int id);
}
