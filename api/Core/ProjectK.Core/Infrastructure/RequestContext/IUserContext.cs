namespace ProjectK.Core.Infrastructure.RequestContext;

public interface IUserContext
{
    public Ulid? UserId { get; }

    public void SetUserId(Ulid userId);
}