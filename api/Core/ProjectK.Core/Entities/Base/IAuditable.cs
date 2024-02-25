namespace ProjectK.Core.Entities.Base;

public interface IAuditable
{
    public DateTime CreatedAtUtc { get; }
    public Ulid CreatedBy { get; }
    public DateTime? UpdatedAtUtc { get; }
    public Ulid? UpdatedBy { get; }
}