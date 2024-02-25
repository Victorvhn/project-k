namespace ProjectK.Core.Dtos.v1;

public record PaginatedData<TEntity>(
    IEnumerable<TEntity> Data,
    long TotalCount,
    int PageSize,
    int CurrentPage
);