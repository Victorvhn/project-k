namespace ProjectK.Api.Dtos.v1.Categories.Responses;

/// <summary>
///     Represents the response after creating a category.
/// </summary>
/// <param name="Id">The unique identifier of the category.</param>
/// <param name="Name">The name of the category.</param>
/// <param name="HexColor">The color that the category will be exhibited.</param>
public record CategoryDto(
    Ulid Id,
    string Name,
    string HexColor
);