namespace ProjectK.Core.Entities.Base;

public interface IUserBasedEntity
{
    public Ulid UserId { get; }
    public User? User { get; }
}