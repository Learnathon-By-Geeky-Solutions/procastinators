using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Infrastructure.Repositories;
using FinanceTracker.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<FinanceTrackerDbContext>(option =>
            option.UseLazyLoadingProxies(false).UseSqlServer(connectionString)
        );

        services
            .AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<FinanceTrackerDbContext>();

        services.AddScoped<IFinanceTrackerSeeder, FinanceTrackerSeeder>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPersonalTransactionRepository, PersonalTransactionRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<ILoanRequestRepository, LoanRequestRepository>();
        services.AddScoped<IInstallmentRepository, InstallmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
