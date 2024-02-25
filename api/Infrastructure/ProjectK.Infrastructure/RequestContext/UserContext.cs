using ProjectK.Core.Infrastructure.RequestContext;

namespace ProjectK.Infrastructure.RequestContext;

internal class UserContext : IUserContext
{
    public Ulid? UserId { get; private set; }

    public void SetUserId(Ulid userId)
    {
        UserId = userId;
    }
}