namespace ProjectK.Core.Entities.Base;

public interface IEntityBase<out T>
{
    public T Id { get; }
}