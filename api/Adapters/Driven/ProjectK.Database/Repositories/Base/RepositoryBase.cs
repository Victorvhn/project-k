using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ProjectK.Core.Adapters.Driven.Database.Repositories.Base;
using ProjectK.Core.Entities.Base;
using ProjectK.Database.Contexts;

namespace ProjectK.Database.Repositories.Base;

[ExcludeFromCodeCoverage]
internal class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey> where TEntity : class, IEntityBase<TKey>
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<TEntity> Set;

    protected RepositoryBase(AppDbContext dbContext)
    {
        DbContext = dbContext;
        Set = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await Set.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetByIdsAsync(TKey[] ids, CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(w => ids.Contains(w.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Set.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        Set.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Set.Remove(entity);
    }

    public async Task<bool> ExistsByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .AnyAsync(a => Equals(a.Id, id), cancellationToken);
    }
}