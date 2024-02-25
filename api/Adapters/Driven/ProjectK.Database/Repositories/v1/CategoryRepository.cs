using Microsoft.EntityFrameworkCore;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.Categories;
using ProjectK.Database.Contexts;
using ProjectK.Database.Repositories.Base;

namespace ProjectK.Database.Repositories.v1;

internal class CategoryRepository(AppDbContext dbContext)
    : RepositoryBase<Category, Ulid>(dbContext), ICategoryRepository
{
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Set.AnyAsync(
            s => s.Name.ToLower() == name.ToLower(),
            cancellationToken);
    }

    public async Task<(IEnumerable<Category> categories, long totalCount)> GetPaginatedAsync(
        GetPaginatedCategoriesQuery query,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await Set.CountAsync(cancellationToken);

        var categories = await Set
            .OrderBy(o => o.Name)
            .Skip((query.CurrentPage - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return (categories, totalCount);
    }
}