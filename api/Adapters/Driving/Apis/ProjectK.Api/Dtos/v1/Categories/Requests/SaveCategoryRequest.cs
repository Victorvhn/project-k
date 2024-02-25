namespace ProjectK.Api.Dtos.v1.Categories.Requests;

/// <summary>
///     Represents the necessary request to create a category.
/// </summary>
/// <param name="Name">The name of the category.</param>
/// <param name="HexColor">The color that the category will be exhibited.</param>
public record SaveCategoryRequest(
    string Name,
    string? HexColor = null
);