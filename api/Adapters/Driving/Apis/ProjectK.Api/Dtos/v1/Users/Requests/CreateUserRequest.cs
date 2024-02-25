namespace ProjectK.Api.Dtos.v1.Users.Requests;

/// <summary>
///     Represents the necessary request to create an user.
/// </summary>
/// <param name="Name">The name of the user.</param>
/// <param name="Email">The email address of the user.</param>
public record CreateUserRequest(
    string Name,
    string Email
);