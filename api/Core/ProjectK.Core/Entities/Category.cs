#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// ReSharper disable CollectionNeverUpdated.Global

using Humanizer;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Core.Entities.Base;

namespace ProjectK.Core.Entities;

public class Category : EntityBase, IUserBasedEntity, IAuditable
{
    public const string DefaultColor = "#D1D5DB";

    protected Category()
    {
    }

    private Category(string name, string hexColor)
    {
        Name = name.Transform(To.SentenceCase);
        HexColor = hexColor;
    }

    public string Name { get; private set; }

    public string HexColor { get; private set; }

    public ICollection<PlannedTransaction> PlannedTransactions { get; } = new List<PlannedTransaction>();
    public ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public Ulid UserId { get; }
    public User? User { get; }
    
    public DateTime CreatedAtUtc { get; }
    public Ulid CreatedBy { get; }
    public DateTime? UpdatedAtUtc { get; }
    public Ulid? UpdatedBy { get; }

    public static Category CreateInstance(CreateCategoryCommand command)
    {
        return new Category(command.Name, command.HexColor ?? DefaultColor);
    }

    public void Update(UpdateCategoryCommand command)
    {
        Name = command.Name.Transform(To.SentenceCase);
        HexColor = command.HexColor ?? DefaultColor;
    }

    public static class Constraints
    {
        public const int NameMaxLength = 50;
    }
}