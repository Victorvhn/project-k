using ProjectK.Core.Enums;

namespace ProjectK.Core.Commands.v1.PlannedTransactions.Update;

public sealed record UpdatePlannedTransactionCommand(
    Ulid PlannedTransactionId,
    string Description,
    decimal Amount,
    AmountType AmountType,
    TransactionType Type,
    Recurrence Recurrence,
    DateOnly StartsAt,
    DateOnly? EndsAt,
    Ulid? CategoryId
) : UpdatePlannedTransactionCommandBase(PlannedTransactionId, Description, Amount);