using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Dtos.v1.PlannedTransactions;

public record PlannedTransactionDto(
    Ulid Id,
    string Description,
    decimal Amount,
    AmountType AmountType,
    TransactionType Type,
    Recurrence Recurrence,
    DateOnly StartsAt,
    DateOnly? EndsAt,
    Ulid? CategoryId,
    bool IsDataFromCustomPlannedTransaction = false,
    bool IsThereAnyCustomPlannedTransaction = false
)
{
    public static PlannedTransactionDto CreateInstance(PlannedTransaction plannedTransaction, bool isThereAnyCustomPlannedTransaction = false)
    {
        return new PlannedTransactionDto(
            plannedTransaction.Id,
            plannedTransaction.Description,
            plannedTransaction.Amount,
            plannedTransaction.AmountType,
            plannedTransaction.Type,
            plannedTransaction.Recurrence,
            plannedTransaction.StartsAt,
            plannedTransaction.EndsAt,
            plannedTransaction.CategoryId,
            false,
            isThereAnyCustomPlannedTransaction
        );
    }

    public static PlannedTransactionDto CreateInstance(PlannedTransaction plannedTransaction,
        CustomPlannedTransaction customPlannedTransaction)
    {
        return new PlannedTransactionDto(
            plannedTransaction.Id,
            customPlannedTransaction.Description,
            customPlannedTransaction.Amount,
            plannedTransaction.AmountType,
            plannedTransaction.Type,
            plannedTransaction.Recurrence,
            customPlannedTransaction.RefersTo,
            customPlannedTransaction.RefersTo,
            plannedTransaction.CategoryId,
            true,
            true
        );
    }
}