namespace ProjectK.Core.Commands.v1.PlannedTransactions.Update;

public sealed record UpdateMonthlyPlannedTransactionCommand(
    Ulid PlannedTransactionId,
    string Description,
    decimal Amount,
    DateOnly StartsAt,
    int Year,
    int Month
) : UpdatePlannedTransactionCommandBase(PlannedTransactionId, Description, Amount);