using ProjectK.Core.Enums;

namespace ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;

/// <summary>
///     Represents a request structure used for updating planned transactions.
/// </summary>
/// <param name="ActionType">Specifies the action type to be performed.</param>
/// <param name="Year">Specifies the year to retrieve the planned transactions.</param>
/// <param name="Month">Specifies the month to retrieve the planned transactions.</param>
/// <param name="Description">Describes the transaction briefly or provides additional information.</param>
/// <param name="Amount">Specifies the monetary value associated with the transaction.</param>
/// <param name="AmountType">Enumerated type defining the nature of the transaction amount.</param>
/// <param name="Type">Enumerated type defining the type of the transaction.</param>
/// <param name="Recurrence">Describes the frequency of the transaction.</param>
/// <param name="StartsAt">Indicates the start date of the planned transaction.</param>
/// <param name="EndsAt">Represents the optional end date for a recurring transaction. Null if it's a no-end transaction.</param>
/// <param name="CategoryId">Identifies the category which the transaction belongs. Null if unspecified.</param>
public record UpdatePlannedTransactionRequest(
    // ActionType is here instead of in query params because it's easier to auto validate with fluent
    ActionType ActionType,
    // Year is here instead of in query params because it's easier to auto validate with fluent
    int Year,
    // Month is here instead of in query params because it's easier to auto validate with fluent
    int Month,
    string Description,
    decimal Amount,
    AmountType AmountType,
    TransactionType Type,
    Recurrence Recurrence,
    DateOnly StartsAt,
    DateOnly? EndsAt,
    Ulid? CategoryId
);