using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
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

    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusNotFound()
    {
        // Arrange

        var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var middleware = new ExceptionHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var notFoundException = new NotFoundException(nameof(Wallet), "1");

        // Act

        await middleware.InvokeAsync(context, _ => throw notFoundException);

        // Assert

        context.Response.StatusCode.Should().Be(404);
    }

    [Fact()]
    public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusForbidden()
    {
        // Arrange

        var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var middleware = new ExceptionHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new ForbiddenException();

        // Act

        await middleware.InvokeAsync(context, _ => throw exception);

        // Assert

        context.Response.StatusCode.Should().Be(403);
    }

    [Fact()]
    public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusInternalServerError()
    {
        // Arrange

        var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var middleware = new ExceptionHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new Exception();

        // Act

        await middleware.InvokeAsync(context, _ => throw exception);

        // Assert

        context.Response.StatusCode.Should().Be(500);
    }
}
