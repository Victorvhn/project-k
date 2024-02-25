using ProjectK.Core.Enums;

namespace ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;

/// <summary>
///     Represents a request structure used for saving planned transactions.
/// </summary>
/// <param name="Description">Describes the transaction briefly or provides additional information.</param>
/// <param name="Amount">Specifies the monetary value associated with the transaction.</param>
/// <param name="AmountType">Enumerated type defining the nature of the transaction amount.</param>
/// <param name="Type">Enumerated type defining the type of the transaction.</param>
/// <param name="Recurrence">Describes the frequency of the transaction.</param>
/// <param name="StartsAt">Indicates the start date of the planned transaction.</param>
/// <param name="EndsAt">Represents the optional end date for a recurring transaction. Null if it's a no-end transaction.</param>
/// <param name="CategoryId">Identifies the category which the transaction belongs. Null if unspecified.</param>
public record CreatePlannedTransactionRequest(
    string Description,
    decimal Amount,
    AmountType AmountType,
    TransactionType Type,
    Recurrence Recurrence,
    DateOnly? StartsAt,
    DateOnly? EndsAt,
    Ulid? CategoryId
);