using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectK.Core.Entities.Base;
using ProjectK.Core.Infrastructure.RequestContext;

namespace ProjectK.Database.Interceptors;

public class ApplyAuditInfoInterceptor(IUserContext userContext) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (userContext.UserId is null || eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.ChangeTracker
            .Entries()
            .Where(e => e is { Entity: IAuditable, State: EntityState.Added or EntityState.Modified })
            .ToList();

        foreach (var entry in entries.Where(w => w.State == EntityState.Added))
        {
            entry.Property("CreatedAtUtc").CurrentValue = DateTime.UtcNow;
            entry.Property("CreatedBy").CurrentValue = userContext.UserId ?? Ulid.Empty;
        }
        
        foreach (var entry in entries.Where(w => w.State == EntityState.Modified))
        {
            entry.Property("UpdatedAtUtc").CurrentValue = DateTime.UtcNow;
            entry.Property("UpdatedBy").CurrentValue = userContext.UserId ?? Ulid.Empty;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}