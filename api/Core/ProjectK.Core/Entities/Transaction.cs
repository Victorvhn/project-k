#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities.Base;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Entities;

public class Transaction : EntityBase, IUserBasedEntity, IAuditable
{
    protected Transaction()
    {
        // EF Core
    }

    private Transaction(string description, decimal amount, TransactionType type,
        DateOnly paidAt, Ulid? categoryId, Ulid? plannedTransactionId)
    {
        Description = description;
        Amount = amount;
        Type = type;
        PaidAt = paidAt;
        CategoryId = categoryId;
        PlannedTransactionId = plannedTransactionId;
    }

    public string Description { get; private set; }
    public decimal Amount { get; private set; }

    public TransactionType Type { get; private set; }

    public DateOnly PaidAt { get; private set; }

    public Ulid? CategoryId { get; private set; }
    public Category? Category { get; }

    public Ulid? PlannedTransactionId { get; private set; }
    public PlannedTransaction? PlannedTransaction { get; }

    public Ulid UserId { get; }
    public User? User { get; }
    
    public DateTime CreatedAtUtc { get; }
    public Ulid CreatedBy { get; }
    public DateTime? UpdatedAtUtc { get; }
    public Ulid? UpdatedBy { get; }

    public static Transaction CreateInstance(CreateTransactionCommand command)
    {
        return new Transaction(
            command.Description,
            command.Amount,
            command.Type,
            command.PaidAt,
            command.CategoryId,
            command.PlannedTransactionId
        );
    }

    public void Update(UpdateTransactionCommand command)
    {
        Description = command.Description;
        Amount = command.Amount;
        Type = command.Type;
        PaidAt = command.PaidAt;
        CategoryId = command.CategoryId;
        PlannedTransactionId = command.PlannedTransactionId;
    }

    public static Transaction CreatePayment(PlannedTransaction plannedTransaction, MonthlyFilter monthlyFilter, decimal amount)
    {
        var isThereACustomPlannedTransaction = plannedTransaction.CustomPlannedTransactions.Count != 0;

        return new Transaction(
            isThereACustomPlannedTransaction
                ? plannedTransaction.CustomPlannedTransactions.First().Description
                : plannedTransaction.Description,
            amount,
            plannedTransaction.Type,
            GetPaymentDate(plannedTransaction, monthlyFilter),
            plannedTransaction.CategoryId,
            plannedTransaction.Id
        );
    }

    private static DateOnly GetPaymentDate(PlannedTransaction plannedTransaction, MonthlyFilter monthlyFilter)
    {
        var now = DateTime.UtcNow;

        var isCurrentDayAvailable = now.Day <= DateTime.DaysInMonth(monthlyFilter.Year, monthlyFilter.Month);

        if (isCurrentDayAvailable)
        {
            return new DateOnly(monthlyFilter.Year, monthlyFilter.Month, now.Day);
        }

        return new DateOnly(monthlyFilter.Year, monthlyFilter.Month,
            DayOnlySafe.Get(monthlyFilter, plannedTransaction.StartsAt.Day));
    }

    public static class Constraints
    {
        public const int DescriptionMaxLength = 150;
    }
}