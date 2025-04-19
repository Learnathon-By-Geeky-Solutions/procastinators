using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceTracker.Api.Middlewares.Tests;

public class ExceptionHandlingMiddlewareTests
{
    [Fact()]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // Arrange

        var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var middleware = new ExceptionHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        // Act

        await middleware.InvokeAsync(context, nextDelegateMock.Object);

        // Assert

        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }
}