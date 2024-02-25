using Mediator;

namespace ProjectK.Core.Commands.v1.Transactions;

public sealed record DeleteTransactionCommand(Ulid TransactionId) : ICommand;