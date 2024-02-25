#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ProjectK.Core.Entities.Base;

public abstract class EntityBase : IEntityBase<Ulid>
{
    public Ulid Id { get; }
}