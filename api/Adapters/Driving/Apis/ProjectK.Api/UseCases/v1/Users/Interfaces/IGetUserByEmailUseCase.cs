using ProjectK.Api.Dtos.v1.Users.Responses;

namespace ProjectK.Api.UseCases.v1.Users.Interfaces;

public interface IGetUserByEmailUseCase
{
    Task<UserDto?> ExecuteAsync(string email, CancellationToken cancellationToken = default);
}