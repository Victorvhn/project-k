#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ProjectK.Core.Commands.v1.PlannedTransactions;
using ProjectK.Core.Commands.v1.PlannedTransactions.Update;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities.Base;
using ProjectK.Core.Enums;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Entities;

public class PlannedTransaction : EntityBase, IUserBasedEntity, IAuditable
{
    protected PlannedTransaction()
    {
        // EF Core
    }

    private PlannedTransaction(string description)
    {
        Description = description;
    }

    public string Description { get; private set; }
    public decimal Amount { get; private set; }

    public AmountType AmountType { get; private set; }
    public TransactionType Type { get; private set; }
    public Recurrence Recurrence { get; private set; }

    public DateOnly StartsAt { get; private set; }
    public DateOnly? EndsAt { get; private set; }

    public Ulid? CategoryId { get; private set; }
    public Category? Category { get; }
    
    public ICollection<CustomPlannedTransaction> CustomPlannedTransactions { get; } =
        new List<CustomPlannedTransaction>();

    public ICollection<Transaction> Transactions { get; } =
        new List<Transaction>();

    public Ulid UserId { get; }
    public User? User { get; }

    public DateTime CreatedAtUtc { get; }
    public Ulid CreatedBy { get; }
    public DateTime? UpdatedAtUtc { get; }
    public Ulid? UpdatedBy { get; }
    
    public static PlannedTransaction CreateInstance(CreatePlannedTransactionCommand data)
    {
        return new PlannedTransaction(data.Description)
        {
            Amount = data.Amount,
            AmountType = data.AmountType,
            Type = data.Type,
            Recurrence = data.Recurrence,
            StartsAt = data.StartsAt!.Value,
            EndsAt = data.EndsAt,
            CategoryId = data.CategoryId
        };
    }

    public void Update(UpdatePlannedTransactionCommand command)
    {
        Description = command.Description;
        Amount = command.Amount;
        AmountType = command.AmountType;
        Type = command.Type;
        Recurrence = command.Recurrence;
        StartsAt = command.StartsAt;
        EndsAt = command.EndsAt;
        CategoryId = command.CategoryId;
    }

    public void Update(UpdateFromNowOnPlannedTransactionCommand command, MonthlyFilter filter)
    {
        Description = command.Description;
        Amount = command.Amount;
        AmountType = command.AmountType;
        Type = command.Type;
        Recurrence = command.Recurrence;
        StartsAt = new DateOnly(StartsAt.Year, StartsAt.Month, DayOnlySafe.Get(filter, command.StartsAt.Day));
        EndsAt = command.EndsAt;
        CategoryId = command.CategoryId;
    }

    public void AddCustomPlannedTransaction(CustomPlannedTransaction customPlannedTransaction)
    {
        CustomPlannedTransactions.Add(customPlannedTransaction);
    }

    public void Inactivate(MonthlyFilter monthlyFilter)
    {
        var isDayAvailable = StartsAt.Day <= DateTime.DaysInMonth(monthlyFilter.Year, monthlyFilter.Month);

        EndsAt = new DateOnly(monthlyFilter.Year, monthlyFilter.Month,
                isDayAvailable ? StartsAt.Day : DateTime.DaysInMonth(monthlyFilter.Year, monthlyFilter.Month))
            .AddMonths(-1);
    }

    public CustomPlannedTransaction? GetCustomPlannedTransactionBetweenDates(DateOnly startOfTheMonth,
        DateOnly endOfTheMonth)
    {
        return CustomPlannedTransactions.SingleOrDefault(w =>
            w.RefersTo >= startOfTheMonth && w.RefersTo <= endOfTheMonth);
    }

    public string[] GetTags()
    {
        var tags = new List<string>
        {
            Resources.ResourceManager.GetString(
                $"TransactionType_{Enum.GetName(typeof(TransactionType), Type)!}")!,
            Resources.ResourceManager.GetString(
                $"Recurrence_{Enum.GetName(typeof(Recurrence), Recurrence)!}")!,
            Resources.ResourceManager.GetString(
                $"AmountType_{Enum.GetName(typeof(AmountType), AmountType)!}")!
        };

        if (Category is not null)
        {
            tags.Add(Category.Name);
        }

        if (Transactions.Count != 0)
        {
            var amount = CustomPlannedTransactions.Count != 0
                ? CustomPlannedTransactions.Sum(s => s.Amount)
                : Amount;

            if (Transactions.Sum(s => s.Amount) > amount)
                tags.Add(Resources.Overpaid);
            else if (Transactions.Sum(s => s.Amount) < amount) tags.Add(Resources.Underpaid);
        }

        if (CustomPlannedTransactions.Count != 0)
        {
            tags.Add(Resources.CustomPlannedTransaction);
        }
        
        return tags.ToArray();
    }

    public static class Constraints
    {
        public const int DescriptionMaxLength = 50;
    }
}