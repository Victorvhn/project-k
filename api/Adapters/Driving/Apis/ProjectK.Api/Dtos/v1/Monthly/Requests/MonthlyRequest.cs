using Microsoft.AspNetCore.Mvc;

namespace ProjectK.Api.Dtos.v1.Monthly.Requests;

/// <summary>
///     Represents a request structure used for monthly data retrieval based on the year and month.
/// </summary>
/// <param name="Year">The year for which the data is requested.</param>
/// <param name="Month">The month for which the data is requested.</param>
public record MonthlyRequest(
    [property: BindProperty(Name = "year")]
    int Year,
    [property: BindProperty(Name = "month")]
    int Month
);