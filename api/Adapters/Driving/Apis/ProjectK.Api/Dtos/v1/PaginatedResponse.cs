namespace ProjectK.Api.Dtos.v1;

/// <summary>
///     Represents a paginated response containing a subset of data along with pagination details.
/// </summary>
/// <typeparam name="TEntity">The type of data in the paginated response.</typeparam>
/// <param name="Data">The data of the paginated response.</param>
/// <param name="TotalCount">The total count of items available across all pages.</param>
/// <param name="PageSize">The maximum number of items to display in a single page.</param>
/// <param name="CurrentPage">The current page number.</param>
public record PaginatedResponse<TEntity>(
    IEnumerable<TEntity> Data,
    long TotalCount,
    int PageSize,
    int CurrentPage
)
{
    /// <summary>
    ///     The total number of pages based on the provided page size and total count.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (decimal)PageSize);

    /// <summary>
    ///     Indicates whether there is a next page available based on the current page and total pages.
    /// </summary>
    public bool HasNext => CurrentPage < TotalPages;

    /// <summary>
    ///     Indicates whether there is a previous page available based on the current page and total count.
    /// </summary>
    public bool HasPrevious => TotalCount > 0L && CurrentPage > 1 && CurrentPage <= TotalPages;
}