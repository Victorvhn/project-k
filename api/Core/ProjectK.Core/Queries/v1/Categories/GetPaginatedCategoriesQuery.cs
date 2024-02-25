using Mediator;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Queries.v1.Categories;

public sealed record GetPaginatedCategoriesQuery(
    int CurrentPage = 1,
    int PageSize = 10
) : IQuery<PaginatedData<Category>>;