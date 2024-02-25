using Mediator;
using ProjectK.Api.UseCases.v1.Users.Interfaces;
using ProjectK.Core.Commands.v1.Users;

namespace ProjectK.Api.UseCases.v1.Users;

internal class DeleteUserUseCase(ISender mediator) : IDeleteUserUseCase
{
    public async Task ExecuteAsync(Ulid userId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserCommand(userId);

        await mediator.Send(command, cancellationToken);
    }
}