using Mediator;

namespace ProjectK.Core.Commands.v1.PlannedTransactions;

public sealed record PayPlannedTransactionCommand(
    Ulid PlannedTransactionId,
    int Year,
    int Month,
    decimal Amount
) : ICommand;