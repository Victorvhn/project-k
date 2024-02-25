using ProjectK.Api.Dtos.v1.Users.Requests;
using ProjectK.Api.Dtos.v1.Users.Responses;

namespace ProjectK.Api.UseCases.v1.Users.Interfaces;

public interface ICreateUserUseCase
{
    Task<UserDto?> ExecuteAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
}