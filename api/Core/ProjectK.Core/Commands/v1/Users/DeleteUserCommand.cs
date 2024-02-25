using Mediator;

namespace ProjectK.Core.Commands.v1.Users;

public sealed record DeleteUserCommand(Ulid UserId) : ICommand;