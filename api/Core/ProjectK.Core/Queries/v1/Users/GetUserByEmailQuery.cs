using Mediator;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Queries.v1.Users;

public sealed record GetUserByEmailQuery(string Email) : IQuery<User?>;