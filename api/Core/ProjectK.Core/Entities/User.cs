#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// ReSharper disable CollectionNeverUpdated.Global

using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Entities.Base;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Entities;

public class User : EntityBase
{
    protected User()
    {
        // EF Core
    }

    private User(string name, string email)
    {
        Name = name;
        Email = email;
        Currency = Currency.Brl;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public Currency Currency { get; private set; }

    public ICollection<Category> Categories { get; } = new List<Category>();
    public ICollection<PlannedTransaction> PlannedTransactions { get; } = new List<PlannedTransaction>();

    public ICollection<CustomPlannedTransaction> CustomPlannedTransactions { get; } =
        new List<CustomPlannedTransaction>();

    public ICollection<Transaction> Transactions { get; } =
        new List<Transaction>();

    public static User CreateInstance(CreateUserCommand command)
    {
        return new User(command.Name, command.Email);
    }

    public static class Constraints
    {
        public const int NameMaxLength = 200;
        public const int EmailMaxLength = 320;
    }

    public void UpdateCurrency(Currency currency)
    {
        Currency = currency;
    }
}