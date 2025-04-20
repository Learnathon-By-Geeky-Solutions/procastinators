using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace FinanceTracker.Application.Users.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetUser_WithAuthenticatedUser_ShouldReturnUser()
    {
        // arrange

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier,"1"),
            new(ClaimTypes.Email,"test@test.com")
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });

        var userContext = new UserContext(httpContextAccessorMock.Object);

        // act

        var currentUser = userContext.GetUser();

        // assert

        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
    }

    [Fact()]
    public void GetUser_WithUserContextNotPresent_ThrowInvalidOperation()
    {
        // arrange

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        var userContext = new UserContext(httpContextAccessorMock.Object);

        // act

        Action action = () => userContext.GetUser();

        // assert

        action.Should().
            Throw<InvalidOperationException>()
            .WithMessage("User context is not available");
    }

    [Fact]
    public void GetUser_WithUnauthenticatedUser_ReturnsNull()
    {
        // arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var unauthenticatedIdentity = new ClaimsIdentity(); // No authentication type = unauthenticated
        var user = new ClaimsPrincipal(unauthenticatedIdentity);
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
        {
            User = user
        });
        var userContext = new UserContext(httpContextAccessorMock.Object);

        // act
        var result = userContext.GetUser();

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetUser_WithNullIdentity_ReturnsNull()
    {
        // arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userMock = new Mock<ClaimsPrincipal>();
        userMock.Setup(u => u.Identity).Returns((IIdentity?)null);

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
        {
            User = userMock.Object
        });
        var userContext = new UserContext(httpContextAccessorMock.Object);

        // act
        var result = userContext.GetUser();

        // assert
        result.Should().BeNull();
    }
}