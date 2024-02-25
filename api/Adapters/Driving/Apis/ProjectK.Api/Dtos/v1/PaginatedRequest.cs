namespace ProjectK.Api.Dtos.v1;

/// <summary>
///     Represents pagination parameters for retrieving paginated data.
/// </summary>
public class PaginatedRequest
{
    /// <summary>
    ///     The number of items per page. Default is 10.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    ///     The current page number. Default is 1.
    /// </summary>
    public int CurrentPage { get; set; } = 1;
}