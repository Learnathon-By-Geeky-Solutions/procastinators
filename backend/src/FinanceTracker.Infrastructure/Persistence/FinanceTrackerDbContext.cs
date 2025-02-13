using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence;

public class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) 
    : DbContext(options)
{

}
