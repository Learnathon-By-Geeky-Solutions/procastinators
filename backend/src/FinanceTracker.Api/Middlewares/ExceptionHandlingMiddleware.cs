
using FinanceTracker.Domain.Exceptions;
using System.Net;

namespace FinanceTracker.Api.Middlewares;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException exception)
        {
            logger.LogError(exception, exception.Message);
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (ForbiddenException exception)
        {
            logger.LogError(exception, exception.Message);
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}
