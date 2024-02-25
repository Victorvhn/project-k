using ProjectK.Core.Adapters.Driven.Database.Repositories.Base;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Adapters.Driven.Database.Repositories.v1;

public interface ITransactionRepository : IRepositoryBase<Transaction, Ulid>
{
    Task<decimal> GetCurrentAmountSummaryByDateAsync(MonthlyFilter filter, TransactionType transactionType,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Transaction>> GetMonthlyAsync(MonthlyFilter filter, CancellationToken cancellationToken = default);
}