using ProjectK.Core.Enums;

namespace ProjectK.Api.Dtos.v1.Transactions.Requests;

/// <summary>
///     Represents a request structure used for saving transactions.
/// </summary>
/// <param name="Description">Describes the transaction briefly or provides additional information.</param>
/// <param name="Amount">Specifies the monetary value associated with the transaction.</param>
/// <param name="Type">Enumerated type defining the type of the transaction.</param>
/// <param name="PaidAt">Represents the date of the payment.</param>
/// <param name="CategoryId">Identifies the category which the transaction belongs. Null if unspecified.</param>
/// <param name="PlannedTransactionId">
///     Identifies the planned transaction which the transaction belongs. Null if
///     unspecified.
/// </param>
public record SaveTransactionRequest(
    string Description,
    decimal Amount,
    TransactionType Type,
    DateOnly PaidAt,
    Ulid? CategoryId,
    Ulid? PlannedTransactionId
);