using Microsoft.EntityFrameworkCore;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Entities;
using ProjectK.Database.Contexts;
using ProjectK.Database.Repositories.Base;

namespace ProjectK.Database.Repositories.v1;

internal class UserRepository(AppDbContext dbContext) : RepositoryBase<User, Ulid>(dbContext), IUserRepository
{
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Set.AnyAsync(s => s.Email == email, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Set.SingleOrDefaultAsync(s => s.Email == email, cancellationToken);
    }
}