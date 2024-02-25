using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectK.Database.Contexts;

namespace ProjectK.Database.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static async Task<WebApplication> UseDatabase(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Database.MigrateAsync();

        return app;
    }
}