using Bogus;
using Bogus.Extensions;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Tests.Shared.Builders.Entities;

public class PlannedTransactionBuilder
{
    private static readonly Faker Faker = new();

    private readonly ICollection<Transaction> _transactions =
        new List<Transaction>();

    private decimal _amount = Faker.Random.Decimal2();
    private AmountType _amountType = Faker.Random.Enum<AmountType>();
    private Category? _category = CategoryBuilder.Create();

    private ICollection<CustomPlannedTransaction> _customPlannedTransactions =
        new List<CustomPlannedTransaction>();

    private string _description = Faker.Random.String2(1, PlannedTransaction.Constraints.DescriptionMaxLength);

    private DateOnly? _endsAt = Faker.Date.FutureDateOnly();
    private Recurrence _recurrence = Faker.Random.Enum<Recurrence>();
    private DateOnly _startsAt = Faker.Date.PastDateOnly();

    private TransactionType _type = Faker.Random.Enum<TransactionType>();

    public PlannedTransaction Build()
    {
        return BuilderHelper
            .InstantiateClass<PlannedTransaction>()
            .SetPropertyValue(entity => entity.Description, _description)
            .SetPropertyValue(entity => entity.Amount, _amount)
            .SetPropertyValue(entity => entity.AmountType, _amountType)
            .SetPropertyValue(entity => entity.Type, _type)
            .SetPropertyValue(entity => entity.Recurrence, _recurrence)
            .SetPropertyValue(entity => entity.StartsAt, _startsAt)
            .SetPropertyValue(entity => entity.EndsAt, _endsAt)
            .SetPropertyValue(entity => entity.Category, _category)
            .SetPropertyValue(entity => entity.CustomPlannedTransactions, _customPlannedTransactions)
            .SetPropertyValue(entity => entity.Transactions, _transactions);
    }


    public static PlannedTransaction[] CreateMany(int? quantity)
    {
        var categories = new List<PlannedTransaction>();

        quantity ??= Faker.Random.Int(1, 10);

        for (var i = 0; i < quantity; i++)
            categories.Add(BuilderHelper
                .InstantiateClass<PlannedTransaction>()
                .SetPropertyValue(entity => entity.Description,
                    Faker.Random.String2(1, PlannedTransaction.Constraints.DescriptionMaxLength))
                .SetPropertyValue(entity => entity.Amount, Faker.Random.Decimal2())
                .SetPropertyValue(entity => entity.AmountType, Faker.Random.Enum<AmountType>())
                .SetPropertyValue(entity => entity.Type, Faker.Random.Enum<TransactionType>())
                .SetPropertyValue(entity => entity.Recurrence, Faker.Random.Enum<Recurrence>())
                .SetPropertyValue(entity => entity.StartsAt, Faker.Date.PastDateOnly())
                .SetPropertyValue(entity => entity.EndsAt, Faker.Date.FutureDateOnly())
            );

        return categories.ToArray();
    }

    public PlannedTransactionBuilder WithCustomPlannedTransactions(CustomPlannedTransaction custom)
    {
        _customPlannedTransactions.Add(custom);

        return this;
    }

    public PlannedTransactionBuilder WithStartsAt(DateOnly date)
    {
        _startsAt = date;
        return this;
    }

    public PlannedTransactionBuilder WithEndsAt(DateOnly date)
    {
        _endsAt = date;
        return this;
    }

    public PlannedTransactionBuilder WithCategory(Category category)
    {
        _category = category;
        return this;
    }

    public PlannedTransactionBuilder WithRecurrence(Recurrence recurrence)
    {
        _recurrence = recurrence;
        return this;
    }

    public PlannedTransactionBuilder WithoutEndsAt()
    {
        _endsAt = null;
        return this;
    }

    public PlannedTransactionBuilder WithoutCustomPlannedTransactions()
    {
        _customPlannedTransactions = new List<CustomPlannedTransaction>();
        return this;
    }

    public PlannedTransactionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public PlannedTransactionBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public PlannedTransactionBuilder WithTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
        return this;
    }

    public PlannedTransactionBuilder WithType(TransactionType type)
    {
        _type = type;
        return this;
    }

    public PlannedTransactionBuilder WithAmountType(AmountType amountType)
    {
        _amountType = amountType;
        return this;
    }

    public PlannedTransactionBuilder WithoutTransactions()
    {
        _transactions.Clear();
        return this;
    }

    public PlannedTransactionBuilder WithoutCategory()
    {
        _category = null;
        return this;
    }
}