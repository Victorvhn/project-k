using ProjectK.Core.Adapters.Driven.Database.Repositories.Base;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Adapters.Driven.Database.Repositories.v1;

public interface IUserRepository : IRepositoryBase<User, Ulid>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}