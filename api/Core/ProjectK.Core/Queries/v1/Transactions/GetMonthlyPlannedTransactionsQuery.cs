using Mediator;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Queries.v1.Transactions;

public sealed record GetMonthlyTransactionsQuery(
    int Year,
    int Month
) : IQuery<IEnumerable<Transaction>>;