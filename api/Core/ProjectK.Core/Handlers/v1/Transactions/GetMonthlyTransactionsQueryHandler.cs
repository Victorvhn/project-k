using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.Transactions;

namespace ProjectK.Core.Handlers.v1.Transactions;

internal sealed class GetMonthlyTransactionsQueryHandler(
    ITransactionRepository transactionRepository)
    : IQueryHandler<GetMonthlyTransactionsQuery, IEnumerable<Transaction>>
{
    public async ValueTask<IEnumerable<Transaction>> Handle(GetMonthlyTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new MonthlyFilter(request.Year, request.Month);

        return await transactionRepository.GetMonthlyAsync(filter, cancellationToken);
    }
}