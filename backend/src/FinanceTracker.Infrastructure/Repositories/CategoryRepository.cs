using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using System;

namespace FinanceTracker.Infrastructure.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        public Task<int> Create(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAll(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
