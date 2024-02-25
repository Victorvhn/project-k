using Mediator;

namespace ProjectK.Core.Commands.v1.PlannedTransactions.Delete;

public sealed record DeletePlannedTransactionCommand(
    Ulid PlannedTransactionId
) : DeletePlannedTransactionCommandBase(PlannedTransactionId), ICommand;