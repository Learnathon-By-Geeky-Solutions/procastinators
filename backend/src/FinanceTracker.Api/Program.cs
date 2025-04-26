using FinanceTracker.Api.Extensions;
using FinanceTracker.Api.Middlewares;
using FinanceTracker.Application.Extensions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Extensions;
using FinanceTracker.Infrastructure.Seeders;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.AddPresentation();

    var app = builder.Build();

    try
    {
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IFinanceTrackerSeeder>();
        await seeder.SeedAsync();
    }
    catch (Exception exception)
    {
        Log.Fatal(exception, "Failed Database Seeding");
    }

    // Configure the HTTP request pipeline.

    app.UseSerilogRequestLogging();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHttpsRedirection();
    }

    app.MapGroup("api/identity").WithTags("Identity").MapIdentityApi<User>();

    app.UseAuthorization();

    app.MapControllers();

    app.UseCors();

    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Failed Application Startup");
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program
{
    protected Program() { }
}
