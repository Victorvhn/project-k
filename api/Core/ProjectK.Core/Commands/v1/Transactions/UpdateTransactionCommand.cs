using Mediator;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Commands.v1.Transactions;

public sealed record UpdateTransactionCommand(
    Ulid TransactionId,
    string Description,
    decimal Amount,
    TransactionType Type,
    DateOnly PaidAt,
    Ulid? CategoryId,
    Ulid? PlannedTransactionId
) : ICommand<Transaction?>;