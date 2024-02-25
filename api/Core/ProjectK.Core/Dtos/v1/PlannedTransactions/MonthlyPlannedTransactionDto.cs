using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Dtos.v1.PlannedTransactions;

public record MonthlyPlannedTransactionDto
{
    public MonthlyPlannedTransactionDto(PlannedTransaction plannedTransaction, MonthlyFilter filter)
    {
        var customPlannedTransaction = plannedTransaction.CustomPlannedTransactions.FirstOrDefault();
        var transaction = plannedTransaction.Transactions.FirstOrDefault();

        Id = plannedTransaction.Id;
        Description = customPlannedTransaction?.Description ?? plannedTransaction.Description;
        Amount = customPlannedTransaction?.Amount ?? plannedTransaction.Amount;
        AmountType = plannedTransaction.AmountType;
        Type = plannedTransaction.Type;
        Recurrence = plannedTransaction.Recurrence;
        ReferringDate = customPlannedTransaction != null
            ? new DateOnly(filter.Year, filter.Month, DayOnlySafe.Get(filter, customPlannedTransaction.RefersTo.Day))
            : new DateOnly(filter.Year, filter.Month, DayOnlySafe.Get(filter, plannedTransaction.StartsAt.Day));
        StartsAt = plannedTransaction.StartsAt;
        EndsAt = plannedTransaction.EndsAt;
        CategoryId = plannedTransaction.CategoryId;
        CategoryName = plannedTransaction.Category?.Name;
        Paid = transaction is not null;
        PaidTransactionId = transaction?.Id;
        PaidAt = transaction?.PaidAt;
        PaidAmount = transaction?.Amount;
        Tags = plannedTransaction.GetTags();
        Ignore = !customPlannedTransaction?.Active ?? false;
    }

    public Ulid Id { get; init; }
    public string Description { get; init; }
    public decimal Amount { get; init; }
    public AmountType AmountType { get; init; }
    public TransactionType Type { get; init; }
    public Recurrence Recurrence { get; init; }
    public DateOnly ReferringDate { get; init; }
    public DateOnly StartsAt { get; init; }
    public DateOnly? EndsAt { get; init; }
    public Ulid? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public bool Paid { get; init; }
    public Ulid? PaidTransactionId { get; init; }
    public DateOnly? PaidAt { get; init; }
    public decimal? PaidAmount { get; init; }
    public string[] Tags { get; init; }
    public bool Ignore { get; init; }
};