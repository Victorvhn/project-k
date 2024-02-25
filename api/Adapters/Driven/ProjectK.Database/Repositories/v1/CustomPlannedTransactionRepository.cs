using Microsoft.EntityFrameworkCore;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Entities;
using ProjectK.Database.Contexts;
using ProjectK.Database.Repositories.Base;

namespace ProjectK.Database.Repositories.v1;

internal class CustomPlannedTransactionRepository(AppDbContext dbContext)
    : RepositoryBase<CustomPlannedTransaction, Ulid>(dbContext), ICustomPlannedTransactionRepository
{
    public async Task<bool> AnyByPlannedTransactionIdAsync(Ulid plannedTransactionId) =>
        await Set.AnyAsync(a => a.BasePlannedTransactionId == plannedTransactionId);
}