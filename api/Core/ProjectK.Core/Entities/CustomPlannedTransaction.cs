#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ProjectK.Core.Commands.v1.PlannedTransactions.Update;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities.Base;

namespace ProjectK.Core.Entities;

public class CustomPlannedTransaction : EntityBase, IUserBasedEntity, IAuditable
{
    protected CustomPlannedTransaction()
    {
        // EF Core
    }

    private CustomPlannedTransaction(string description, decimal amount, DateOnly refersTo,
        Ulid basePlannedTransactionId, PlannedTransaction? basePlannedTransaction, bool active)
    {
        Description = description;
        Amount = amount;
        RefersTo = refersTo;
        BasePlannedTransactionId = basePlannedTransactionId;
        BasePlannedTransaction = basePlannedTransaction;
        Active = active;
    }

    public string Description { get; private set; }
    public decimal Amount { get; private set; }

    public DateOnly RefersTo { get; private set; }

    public Ulid BasePlannedTransactionId { get; private set; }
    public PlannedTransaction? BasePlannedTransaction { get; private set; }

    public bool Active { get; private set; }

    public Ulid UserId { get; }
    public User? User { get; }
    
    public DateTime CreatedAtUtc { get; }
    public Ulid CreatedBy { get; }
    public DateTime? UpdatedAtUtc { get; }
    public Ulid? UpdatedBy { get; }

    public static CustomPlannedTransaction CreateInstance(PlannedTransaction plannedTransaction,
        UpdateMonthlyPlannedTransactionCommand data, DateOnly refersTo)
    {
        return new CustomPlannedTransaction(data.Description, data.Amount, refersTo, plannedTransaction.Id,
            plannedTransaction, true);
    }

    public static CustomPlannedTransaction CreateInstance(PlannedTransaction plannedTransaction, DateOnly refersTo)
    {
        return new CustomPlannedTransaction(plannedTransaction.Description, plannedTransaction.Amount, refersTo,
            plannedTransaction.Id,
            plannedTransaction, true);
    }

    public static CustomPlannedTransaction CreateInactiveInstance(PlannedTransaction plannedTransaction,
        DateOnly refersTo)
    {
        return new CustomPlannedTransaction(plannedTransaction.Description, 0, refersTo,
            plannedTransaction.Id,
            plannedTransaction, false);
    }

    public void Update(UpdateMonthlyPlannedTransactionCommand command)
    {
        Description = command.Description;
        Amount = command.Amount;
        RefersTo = new DateOnly(command.Year, command.Month,
            DayOnlySafe.Get(new MonthlyFilter(command.Year, command.Month), command.StartsAt.Day));
    }

    public void Update(UpdatePlannedTransactionCommand command)
    {
        Description = command.Description;
        Amount = command.Amount;
    }
    
    public void Inactivate()
    {
        Amount = 0;
        Active = false;
    }

    public static class Constraints
    {
        public const int DescriptionMaxLength = 50;
    }
}