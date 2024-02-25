using Bogus;
using Bogus.Extensions;
using ProjectK.Core.Entities;

namespace ProjectK.Tests.Shared.Builders.Entities;

public class CustomPlannedTransactionBuilder
{
    private static readonly Faker Faker = new();
    private bool _active = true;
    private decimal _amount = Faker.Random.Decimal2();
    private PlannedTransaction? _basePlannedTransaction = new PlannedTransactionBuilder().Build();

    private string _description = Faker.Random.String2(1, PlannedTransaction.Constraints.DescriptionMaxLength);
    private DateOnly _refersTo = Faker.Date.FutureDateOnly();

    public CustomPlannedTransaction Build()
    {
        return BuilderHelper
            .InstantiateClass<CustomPlannedTransaction>()
            .SetPropertyValue(entity => entity.Description, _description)
            .SetPropertyValue(entity => entity.Amount, _amount)
            .SetPropertyValue(entity => entity.RefersTo, _refersTo)
            .SetPropertyValue(entity => entity.BasePlannedTransaction, _basePlannedTransaction)
            .SetPropertyValue(entity => entity.Active, _active);
    }

    public CustomPlannedTransactionBuilder WithRefersTo(DateOnly date)
    {
        _refersTo = date;
        return this;
    }

    public CustomPlannedTransactionBuilder WithBasePlannedTransaction(PlannedTransaction planned)
    {
        _basePlannedTransaction = planned;
        return this;
    }

    public CustomPlannedTransactionBuilder WithoutBasePlannedTransaction()
    {
        _basePlannedTransaction = null;
        return this;
    }

    public CustomPlannedTransactionBuilder Active()
    {
        _active = true;
        return this;
    }

    public CustomPlannedTransactionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public CustomPlannedTransactionBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }
}