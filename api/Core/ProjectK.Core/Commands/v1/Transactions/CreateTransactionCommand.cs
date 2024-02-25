using Mediator;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Commands.v1.Transactions;

public sealed record CreateTransactionCommand(
    string Description,
    decimal Amount,
    TransactionType Type,
    DateOnly PaidAt,
    Ulid? CategoryId,
    Ulid? PlannedTransactionId
) : ICommand<Transaction?>;