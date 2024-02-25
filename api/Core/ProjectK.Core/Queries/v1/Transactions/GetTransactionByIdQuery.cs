using Mediator;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Queries.v1.Transactions;

public sealed record GetTransactionByIdQuery(Ulid TransactionId) : IQuery<Transaction?>;