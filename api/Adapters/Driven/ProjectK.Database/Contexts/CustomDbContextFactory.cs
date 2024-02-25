using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectK.Core.Infrastructure.RequestContext;
using ServiceCollectionExtensions = ProjectK.Database.Extensions.ServiceCollectionExtensions;

namespace ProjectK.Database.Contexts;

[ExcludeFromCodeCoverage]
public class CustomDbContextFactory(IServiceScopeFactory serviceScopeFactory) : IDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var userContext = serviceProvider.GetRequiredService<IUserContext>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder
            .UseNpgsql(ServiceCollectionExtensions.GetDefaultConnectionString(configuration))
            .UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options, userContext);
    }
}