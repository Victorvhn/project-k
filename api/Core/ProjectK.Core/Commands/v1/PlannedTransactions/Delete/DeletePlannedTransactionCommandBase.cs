namespace ProjectK.Core.Commands.v1.PlannedTransactions.Delete;

public abstract record DeletePlannedTransactionCommandBase(
    Ulid PlannedTransactionId
);