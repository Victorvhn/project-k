using Mediator;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Commands.v1.Users;

public sealed record CreateUserCommand(
    string Name,
    string Email
) : ICommand<User?>;