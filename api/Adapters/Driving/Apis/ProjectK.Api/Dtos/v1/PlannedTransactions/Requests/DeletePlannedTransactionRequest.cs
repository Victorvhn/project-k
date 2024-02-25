using ProjectK.Core.Enums;

namespace ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;

/// <summary>
///     Represents a request structure used for deleting planned transactions.
/// </summary>
/// <param name="ActionType">Specifies the action type to be performed.</param>
/// <param name="Year">Specifies the year to retrieve the planned transactions.</param>
/// <param name="Month">Specifies the month to retrieve the planned transactions.</param>
public record DeletePlannedTransactionRequest(
    ActionType ActionType,
    int Year,
    int Month
);