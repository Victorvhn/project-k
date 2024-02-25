using Microsoft.EntityFrameworkCore;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Database.Contexts;
using ProjectK.Database.Repositories.Base;

namespace ProjectK.Database.Repositories.v1;

internal class TransactionRepository(AppDbContext dbContext)
    : RepositoryBase<Transaction, Ulid>(dbContext), ITransactionRepository
{
    public async Task<decimal> GetCurrentAmountSummaryByDateAsync(MonthlyFilter filter, TransactionType transactionType,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .Where(w => w.PaidAt >= filter.StartOfTheMonth &&
                        w.PaidAt <= filter.EndOfTheMonth)
            .Where(w => w.Type == transactionType)
            .SumAsync(s => s.Amount, cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetMonthlyAsync(MonthlyFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .Include(i => i.PlannedTransaction)
            .Where(w => w.PaidAt >= filter.StartOfTheMonth &&
                        w.PaidAt <= filter.EndOfTheMonth)
            .OrderByDescending(o => o.PaidAt)
            .ThenByDescending(t => t.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}