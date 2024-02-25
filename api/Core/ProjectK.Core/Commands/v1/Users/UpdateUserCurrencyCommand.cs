using Mediator;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Commands.v1.Users;

public sealed record UpdateUserCurrencyCommand(Ulid UserId, Currency Currency) : ICommand<User?>;