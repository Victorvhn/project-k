using Bogus;
using ProjectK.Core.Entities;
using Currency = ProjectK.Core.Enums.Currency;

namespace ProjectK.Tests.Shared.Builders.Entities;

public class UserBuilder
{
    private static readonly Faker Faker = new();
    private readonly string _email = Faker.Person.Email;
    private readonly string _name = Faker.Person.FullName;
    private readonly Currency _currency = (Currency) Faker.Random.Number();

    public User Build()
    {
        return BuilderHelper
            .InstantiateClass<User>()
            .SetPropertyValue(entity => entity.Name, _name)
            .SetPropertyValue(entity => entity.Email, _email)
            .SetPropertyValue(entity => entity.Currency, _currency);
    }

    public static User Create()
    {
        return BuilderHelper
            .InstantiateClass<User>()
            .SetPropertyValue(entity => entity.Name, Faker.Person.FullName)
            .SetPropertyValue(entity => entity.Email, Faker.Person.Email)
            .SetPropertyValue(entity => entity.Currency, (Currency) Faker.Random.Number());
    }
}