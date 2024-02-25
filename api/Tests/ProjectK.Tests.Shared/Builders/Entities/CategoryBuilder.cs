using Bogus;
using ProjectK.Core.Entities;

namespace ProjectK.Tests.Shared.Builders.Entities;

public class CategoryBuilder
{
    private static readonly Faker Faker = new();
    private readonly string _hexColor = HexColorProvider.Random();
    private Ulid? _id;

    private string _name = Faker.Random.String2(1, Category.Constraints.NameMaxLength);

    public Category Build()
    {
        var category = BuilderHelper
            .InstantiateClass<Category>();

        if (_id != null) category.SetPropertyValue(entity => entity.Id, _id);

        return category
            .SetPropertyValue(entity => entity.Name, _name)
            .SetPropertyValue(entity => entity.HexColor, _hexColor);
    }

    public static Category Create()
    {
        return BuilderHelper
            .InstantiateClass<Category>()
            .SetPropertyValue(entity => entity.Name, Faker.Random.String2(1, 50))
            .SetPropertyValue(entity => entity.HexColor, HexColorProvider.Random());
    }

    public static Category[] CreateMany(int? quantity)
    {
        var categories = new List<Category>();

        quantity ??= Faker.Random.Int(1, 10);

        for (var i = 0; i < quantity; i++)
            categories.Add(BuilderHelper
                .InstantiateClass<Category>()
                .SetPropertyValue(entity => entity.Name, Faker.Random.String2(1, 50))
                .SetPropertyValue(entity => entity.HexColor, HexColorProvider.Random()));

        return categories.ToArray();
    }

    public CategoryBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CategoryBuilder WithId(Ulid id)
    {
        _id = id;
        return this;
    }
}