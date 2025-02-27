﻿using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinanceTracker.Infrastructure.Repositories
{
    internal class CategoryRepository(FinanceTrackerDbContext dbContext) : ICategoryRepository
    {
        public async Task<int> Create(Category category)
        {
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            return category.Id;
        }

        public async Task<IEnumerable<Category>> GetAll(string userId)
        {
            return await dbContext.Categories
           .Where(w => w.UserId == userId)
           .ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return await dbContext.Categories
            .FirstOrDefaultAsync(w => w.Id == id);
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
