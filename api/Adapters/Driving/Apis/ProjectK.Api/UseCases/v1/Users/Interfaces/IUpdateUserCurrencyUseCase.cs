using ProjectK.Api.Dtos.v1.Users.Requests;
using ProjectK.Api.Dtos.v1.Users.Responses;

namespace ProjectK.Api.UseCases.v1.Users.Interfaces;

public interface IUpdateUserCurrencyUseCase
{
    Task<UserDto?> ExecuteAsync(Ulid userId, UpdateCurrencyRequest request, CancellationToken cancellationToken = default);
}