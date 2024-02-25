using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Users.Responses;
using ProjectK.Api.UseCases.v1.Users.Interfaces;
using ProjectK.Core.Queries.v1.Users;

namespace ProjectK.Api.UseCases.v1.Users;

internal class GetUserByEmailUseCase(IMapper mapper, ISender mediator) : IGetUserByEmailUseCase
{
    public async Task<UserDto?> ExecuteAsync(string email, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByEmailQuery(email);

        var result = await mediator.Send(query, cancellationToken);

        return mapper.Map<UserDto?>(result);
    }
}