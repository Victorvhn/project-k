using System.Security.Claims;
using ProjectK.Core.Infrastructure.RequestContext;

namespace ProjectK.Api.Middlewares;

public class UserResolutionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IUserContext userContext)
    {
        var userId = GetUserIdFromRequest(context.Request);

        if (string.IsNullOrWhiteSpace(userId))
        {
            await next(context);
            return;
        }

        userContext.SetUserId(Ulid.Parse(userId));

        await next(context);
    }

    private static string? GetUserIdFromRequest(HttpRequest httpRequest)
    {
        return httpRequest.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}