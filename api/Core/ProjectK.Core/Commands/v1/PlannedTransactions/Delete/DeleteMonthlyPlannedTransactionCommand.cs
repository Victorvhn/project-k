using Mediator;

namespace ProjectK.Core.Commands.v1.PlannedTransactions.Delete;

public sealed record DeleteMonthlyPlannedTransactionCommand(
    Ulid PlannedTransactionId,
    int Year,
    int Month
) : DeletePlannedTransactionCommandBase(PlannedTransactionId), ICommand;