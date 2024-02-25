using ProjectK.Core.Adapters.Driven.Database.Repositories.Base;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.Categories;

namespace ProjectK.Core.Adapters.Driven.Database.Repositories.v1;

public interface ICategoryRepository : IRepositoryBase<Category, Ulid>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<(IEnumerable<Category> categories, long totalCount)> GetPaginatedAsync(GetPaginatedCategoriesQuery query,
        CancellationToken cancellationToken = default);
}