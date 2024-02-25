using Mediator;
using ProjectK.Core.Dtos.v1.PlannedTransactions;

namespace ProjectK.Core.Queries.v1.PlannedTransactions;

public sealed record GetMonthlyPlannedTransactionsQuery(
    int Year,
    int Month
) : IQuery<IEnumerable<MonthlyPlannedTransactionDto>>;