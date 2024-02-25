using ProjectK.Core.Enums;

namespace ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;

/// <summary>
///     Represents the planned transaction.
/// </summary>
/// <param name="Id">Represents the unique identifier for the transaction.</param>
/// <param name="Description">Describes the transaction briefly or provides additional information.</param>
/// <param name="Amount">Specifies the monetary value associated with the transaction.</param>
/// <param name="AmountType">Enumerated type defining the nature of the transaction amount.</param>
/// <param name="Type">Enumerated type defining the type of the transaction.</param>
/// <param name="Recurrence">Describes the frequency of the transaction.</param>
/// <param name="StartsAt">Indicates the start date of the planned transaction.</param>
/// <param name="EndsAt">Represents the optional end date for a recurring transaction. Null if it's a no-end transaction.</param>
/// <param name="CategoryId">Identifies the category or group to which the transaction belongs. Null if unspecified.</param>
/// <param name="IsDataFromCustomPlannedTransaction">
///     Identifies if the provided data is from a planned transaction or from
///     a custom planned transactions.
/// </param>
/// <param name="IsThereAnyCustomPlannedTransaction">
///     Identifies if there is any custom planned transaction for this planned transaction.
/// </param>
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
);