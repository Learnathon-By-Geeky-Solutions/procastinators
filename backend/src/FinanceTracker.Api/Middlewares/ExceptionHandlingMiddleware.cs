using System.Net;
using FinanceTracker.Domain.Exceptions;

namespace FinanceTracker.Api.Middlewares;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    private void LogError(Exception exception)
    {
        const string errorLoggingTemplate = "{ExceptionType}: {ErrorMessage}";
        logger.LogError(
            exception,
            errorLoggingTemplate,
            exception.GetType().Name,
            exception.Message
        );
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException exception)
        {
            LogError(exception);
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (ForbiddenException exception)
        {
            LogError(exception);
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (BadRequestException exception)
        {
            LogError(exception);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (Exception exception)
        {
            LogError(exception);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}
