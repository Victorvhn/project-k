using ProjectK.Core.Adapters.Driven.Database.Repositories.Base;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Adapters.Driven.Database.Repositories.v1;

public interface IPlannedTransactionRepository : IRepositoryBase<PlannedTransaction, Ulid>
{
    Task<IEnumerable<MonthlyPlannedTransactionDto>> GetMonthlyAsync(MonthlyFilter filter,
        CancellationToken cancellationToken = default);

    Task<PlannedTransaction?> GetByIdAndDateWithCustomPlannedAsync(Ulid id, MonthlyFilter monthlyFilter,
        CancellationToken cancellationToken = default);

    Task<PlannedTransaction?> GetByIdWithAllCustomPlannedBeforeDateAsync(Ulid id, MonthlyFilter monthlyFilter,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<PlannedTransaction> plannedTransactions, long totalCount)> GetPaginatedAsync(
        PaginationFilter filter, CancellationToken cancellationToken = default);

    Task<decimal> GetExpectedAmountSummaryByDateAsync(MonthlyFilter filter,
        TransactionType transactionType, CancellationToken cancellationToken = default);

    Task<PlannedTransaction?> GetByIdWithCustomPlannedAsync(Ulid id, CancellationToken cancellationToken = default);
}