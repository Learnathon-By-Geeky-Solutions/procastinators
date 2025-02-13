using FinanceTracker.Api.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddPresentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
