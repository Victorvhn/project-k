using ProjectK.Core.Entities.Base;

namespace ProjectK.Core.Adapters.Driven.Database.Repositories.Base;

public interface IRepositoryBase<TEntity, in TKey> where TEntity : IEntityBase<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetByIdsAsync(TKey[] ids, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<bool> ExistsByIdAsync(TKey id, CancellationToken cancellationToken = default);
}