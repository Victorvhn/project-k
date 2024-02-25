using Bogus;
using Bogus.Extensions;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Tests.Shared.Builders.Entities;

public class TransactionBuilder
{
    private static readonly Faker Faker = new();
    private decimal _amount = Faker.Random.Decimal2();
    private Category? _category = CategoryBuilder.Create();
    private Ulid? _categoryId = Ulid.NewUlid();
    private string _description = Faker.Random.String2(1, PlannedTransaction.Constraints.DescriptionMaxLength);
    private DateOnly _paidAt = Faker.Date.PastDateOnly();
    private PlannedTransaction? _plannedTransaction;
    private TransactionType _type = Faker.Random.Enum<TransactionType>();

    public Transaction Build()
    {
        return BuilderHelper
            .InstantiateClass<Transaction>()
            .SetPropertyValue(entity => entity.Description, _description)
            .SetPropertyValue(entity => entity.Amount, _amount)
            .SetPropertyValue(entity => entity.Type, _type)
            .SetPropertyValue(entity => entity.PaidAt, _paidAt)
            .SetPropertyValue(entity => entity.Category, _category)
            .SetPropertyValue(entity => entity.CategoryId, _categoryId)
            .SetPropertyValue(entity => entity.PlannedTransaction, _plannedTransaction);
    }

    public TransactionBuilder WithPaidAt(DateOnly paidAt)
    {
        _paidAt = paidAt;
        return this;
    }

    public TransactionBuilder WithCategory(Category category)
    {
        _category = category;
        _categoryId = category.Id;
        return this;
    }

    public TransactionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public TransactionBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public TransactionBuilder WithType(TransactionType transactionType)
    {
        _type = transactionType;
        return this;
    }

    public TransactionBuilder WithoutCategory()
    {
        _category = null;
        _categoryId = null;
        return this;
    }

    public TransactionBuilder WithPlannedTransaction(PlannedTransaction plannedTransaction)
    {
        _plannedTransaction = plannedTransaction;
        return this;
    }
}