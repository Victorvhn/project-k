using Microsoft.EntityFrameworkCore;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Database.Contexts;
using ProjectK.Database.QueryExpressions;
using ProjectK.Database.Repositories.Base;

namespace ProjectK.Database.Repositories.v1;

internal class PlannedTransactionRepository(AppDbContext dbContext)
    : RepositoryBase<PlannedTransaction, Ulid>(dbContext), IPlannedTransactionRepository
{
    public async Task<IEnumerable<MonthlyPlannedTransactionDto>> GetMonthlyAsync(MonthlyFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(i => i.Category)
            .Include(i => i.CustomPlannedTransactions
                .Where(w => w.RefersTo >= filter.StartOfTheMonth && w.RefersTo <= filter.EndOfTheMonth))
            .Include(i => i.Transactions
                .Where(w => w.PaidAt >= filter.StartOfTheMonth && w.PaidAt <= filter.EndOfTheMonth))
            .Where(MonthlyFilterExpressions.PlannedTransactions.IsFromDate(filter))
            .OrderBy(o => o.StartsAt.Day)
            .ThenBy(t => t.CreatedAtUtc)
            .Select(s => new MonthlyPlannedTransactionDto(s, filter))
            .ToListAsync(cancellationToken);
    }

    public async Task<PlannedTransaction?> GetByIdAndDateWithCustomPlannedAsync(Ulid id, MonthlyFilter monthlyFilter,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(i =>
                i.CustomPlannedTransactions.Where(w =>
                    w.RefersTo >= monthlyFilter.StartOfTheMonth && w.RefersTo <= monthlyFilter.EndOfTheMonth))
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<PlannedTransaction?> GetByIdWithAllCustomPlannedBeforeDateAsync(Ulid id,
        MonthlyFilter monthlyFilter,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(i => i.CustomPlannedTransactions
                .Where(w => w.RefersTo <= monthlyFilter.EndOfTheMonth))
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<PlannedTransaction> plannedTransactions, long totalCount)> GetPaginatedAsync(
        PaginationFilter filter, CancellationToken cancellationToken = default)
    {
        var totalCount = await Set.CountAsync(cancellationToken);

        var plannedTransactions = await Set
            .OrderBy(o => o.Description)
            .Skip((filter.CurrentPage - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (plannedTransactions, totalCount);
    }

    public async Task<decimal> GetExpectedAmountSummaryByDateAsync(MonthlyFilter filter,
        TransactionType transactionType, CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .Where(MonthlyFilterExpressions.PlannedTransactions.IsFromDate(filter))
            .Where(w => w.Type == transactionType)
            .Select(s => s.CustomPlannedTransactions
                    .Any(a => a.RefersTo >= filter.StartOfTheMonth &&
                              a.RefersTo <= filter.EndOfTheMonth)
                    ? s.CustomPlannedTransactions
                        .Where(w => w.RefersTo >= filter.StartOfTheMonth &&
                                    w.RefersTo <= filter.EndOfTheMonth)
                        .Sum(ss => ss.Amount)
                    : s.Amount
            )
            .SumAsync(cancellationToken);
    }

    public async Task<PlannedTransaction?> GetByIdWithCustomPlannedAsync(Ulid id,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(i => i.CustomPlannedTransactions)
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}