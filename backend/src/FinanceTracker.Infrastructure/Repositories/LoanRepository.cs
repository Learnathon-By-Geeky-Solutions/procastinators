﻿using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

internal class LoanRepository(FinanceTrackerDbContext dbContext) : ILoanRepository
{
    public async Task<int> CreateAsync(Loan loan)
    {
        dbContext.Loans.Add(loan);
        await dbContext.SaveChangesAsync();
        return loan.Id;
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await dbContext.Loans.Include(l => l.LoanRequest).FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await dbContext.Loans.Include(l => l.LoanRequest).ToListAsync();
    }

    public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();
}
