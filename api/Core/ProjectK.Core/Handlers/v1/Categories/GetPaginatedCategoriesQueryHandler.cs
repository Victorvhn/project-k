using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.Categories;

namespace ProjectK.Core.Handlers.v1.Categories;

internal class GetPaginatedCategoriesQueryHandler(
    ICategoryRepository categoryRepository) : IQueryHandler<GetPaginatedCategoriesQuery, PaginatedData<Category>?>
{
    public async ValueTask<PaginatedData<Category>?> Handle(GetPaginatedCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var (categories, totalCount) = await categoryRepository.GetPaginatedAsync(request, cancellationToken);

        return new PaginatedData<Category>(
            categories,
            totalCount,
            CurrentPage: request.CurrentPage,
            PageSize: request.PageSize);
    }
}