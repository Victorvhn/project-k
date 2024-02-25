using ProjectK.Core.Enums;

namespace ProjectK.Core.Commands.v1.PlannedTransactions.Update;

public sealed record UpdateFromNowOnPlannedTransactionCommand(
    Ulid PlannedTransactionId,
    int Year,
    int Month,
    string Description,
    decimal Amount,
    AmountType AmountType,
    TransactionType Type,
    Recurrence Recurrence,
    DateOnly StartsAt,
    DateOnly? EndsAt,
    Ulid? CategoryId
) : UpdatePlannedTransactionCommandBase(PlannedTransactionId, Description, Amount);