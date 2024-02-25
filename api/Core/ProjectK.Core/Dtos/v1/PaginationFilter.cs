namespace ProjectK.Core.Dtos.v1;

public record PaginationFilter(
    int PageSize,
    int CurrentPage
);