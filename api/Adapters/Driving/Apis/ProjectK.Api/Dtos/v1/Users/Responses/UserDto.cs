using ProjectK.Core.Enums;

namespace ProjectK.Api.Dtos.v1.Users.Responses;

/// <summary>
///     Represents an user.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Name">The name of the user.</param>
/// <param name="Email">The email address of the user.</param>
public record UserDto(
    Ulid Id,
    string Name,
    string Email,
    Currency Currency
);