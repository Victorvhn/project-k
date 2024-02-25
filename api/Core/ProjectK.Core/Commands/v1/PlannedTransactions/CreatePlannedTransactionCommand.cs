using Mediator;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Commands.v1.PlannedTransactions;

public sealed record CreatePlannedTransactionCommand(
    string Description,
    decimal Amount,
    AmountType AmountType,
    TransactionType Type,
    Recurrence Recurrence,
    DateOnly? StartsAt,
    DateOnly? EndsAt,
    Ulid? CategoryId
) : ICommand<PlannedTransaction?>;