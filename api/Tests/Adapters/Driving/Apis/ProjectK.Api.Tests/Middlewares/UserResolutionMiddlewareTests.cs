using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using ProjectK.Api.Middlewares;
using ProjectK.Core.Infrastructure.RequestContext;

namespace ProjectK.Api.Tests.Middlewares;

public class UserResolutionMiddlewareTests
{
    [Fact]
    public async Task Invoke_WhenUserIdPresent_ShouldSetUserIdInUserContext()
    {
        // Arrange
        var userContextMock = Substitute.For<IUserContext>();
        var middleware = new UserResolutionMiddleware(context => Task.CompletedTask);

        var context = Substitute.For<HttpContext>();
        var httpRequest = Substitute.For<HttpRequest>();
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Ulid.NewUlid().ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(claimsIdentity);

        httpRequest.HttpContext.Returns(context);
        httpRequest.HttpContext.User.Returns(user);

        context.Request.Returns(httpRequest);

        // Act
        await middleware.Invoke(context, userContextMock);

        // Assert
        userContextMock.Received(1).SetUserId(Arg.Any<Ulid>());
    }

    [Fact]
    public async Task Invoke_WhenUserIdNotPresent_ShouldNotSetUserIdInUserContext()
    {
        // Arrange
        var userContextMock = Substitute.For<IUserContext>();
        var middleware = new UserResolutionMiddleware(context => Task.CompletedTask);

        var context = Substitute.For<HttpContext>();
        var httpRequest = Substitute.For<HttpRequest>();
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        httpRequest.HttpContext.Returns(context);
        httpRequest.HttpContext.User.Returns(user);

        context.Request.Returns(httpRequest);

        // Act
        await middleware.Invoke(context, userContextMock);

        // Assert
        userContextMock.DidNotReceive().SetUserId(Arg.Any<Ulid>());
    }
}