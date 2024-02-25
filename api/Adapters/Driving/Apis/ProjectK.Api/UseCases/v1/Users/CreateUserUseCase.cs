using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Users.Requests;
using ProjectK.Api.Dtos.v1.Users.Responses;
using ProjectK.Api.UseCases.v1.Users.Interfaces;
using ProjectK.Core.Commands.v1.Users;

namespace ProjectK.Api.UseCases.v1.Users;

internal class CreateUserUseCase(IMapper mapper, ISender mediator) : ICreateUserUseCase
{
    public async Task<UserDto?> ExecuteAsync(CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateUserCommand(request.Name, request.Email);

        var result = await mediator.Send(command, cancellationToken);

        return mapper.Map<UserDto>(result);
    }
}