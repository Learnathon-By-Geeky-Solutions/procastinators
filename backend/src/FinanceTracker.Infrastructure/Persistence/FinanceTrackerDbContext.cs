using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence;

public class FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) 
    : IdentityDbContext<User>(options)
{

}
