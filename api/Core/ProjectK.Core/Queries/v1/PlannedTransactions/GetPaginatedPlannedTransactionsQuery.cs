using Mediator;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Queries.v1.PlannedTransactions;

public sealed record GetPaginatedPlannedTransactionsQuery(
    int CurrentPage = 1,
    int PageSize = 10
) : IQuery<PaginatedData<PlannedTransaction>>;