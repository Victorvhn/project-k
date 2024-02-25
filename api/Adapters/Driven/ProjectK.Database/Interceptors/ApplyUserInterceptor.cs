using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectK.Core.Entities.Base;
using ProjectK.Core.Infrastructure.RequestContext;

namespace ProjectK.Database.Interceptors;

public class ApplyUserInterceptor(IUserContext userContext) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (userContext.UserId is null || eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.ChangeTracker
            .Entries()
            .Where(e => e is { Entity: IUserBasedEntity, State: EntityState.Added });

        foreach (var entry in entries)
            entry.Property("UserId").CurrentValue = userContext.UserId;

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}