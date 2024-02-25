using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.PlannedTransactions;

namespace ProjectK.Core.Handlers.v1.PlannedTransactions;

internal sealed class GetPaginatedPlannedTransactionsQueryHandler(
    IPlannedTransactionRepository plannedTransactionRepository)
    : IQueryHandler<GetPaginatedPlannedTransactionsQuery, PaginatedData<PlannedTransaction>>
{
    public async ValueTask<PaginatedData<PlannedTransaction>> Handle(GetPaginatedPlannedTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new PaginationFilter(request.PageSize, request.CurrentPage);

        var (plannedTransactions, totalCount) =
            await plannedTransactionRepository.GetPaginatedAsync(filter, cancellationToken);

        return new PaginatedData<PlannedTransaction>(
            plannedTransactions,
            totalCount,
            CurrentPage: filter.CurrentPage,
            PageSize: filter.PageSize);
    }
}