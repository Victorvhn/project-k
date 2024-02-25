using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Queries.v1.PlannedTransactions;

namespace ProjectK.Core.Handlers.v1.PlannedTransactions;

internal sealed class GetMonthlyPlannedTransactionsQueryHandler(
    IPlannedTransactionRepository plannedTransactionRepository)
    : IQueryHandler<GetMonthlyPlannedTransactionsQuery, IEnumerable<MonthlyPlannedTransactionDto>>
{
    public async ValueTask<IEnumerable<MonthlyPlannedTransactionDto>> Handle(GetMonthlyPlannedTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new MonthlyFilter(request.Year, request.Month);

        var plannedTransactions = await plannedTransactionRepository.GetMonthlyAsync(filter, cancellationToken);

        return plannedTransactions;
    }
}