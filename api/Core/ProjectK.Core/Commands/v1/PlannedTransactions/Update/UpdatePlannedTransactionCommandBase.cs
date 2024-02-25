using Mediator;
using ProjectK.Core.Dtos.v1.PlannedTransactions;

namespace ProjectK.Core.Commands.v1.PlannedTransactions.Update;

public abstract record UpdatePlannedTransactionCommandBase(
    Ulid PlannedTransactionId,
    string Description,
    decimal Amount
) : ICommand<PlannedTransactionDto?>;