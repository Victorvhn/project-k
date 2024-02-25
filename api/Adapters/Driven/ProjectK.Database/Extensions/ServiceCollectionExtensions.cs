using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectK.Database.Contexts;
using ProjectK.Database.Interceptors;

namespace ProjectK.Database.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configurations, IHostEnvironment environment)
    {
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(GetDefaultConnectionString(configurations),
                    optionsBuilder => { optionsBuilder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null); })
                .EnableSensitiveDataLogging(environment.IsDevelopment())
                .AddInterceptors(
                    serviceProvider.GetRequiredService<QueryLimitCommandInterceptor>(),
                    serviceProvider.GetRequiredService<ApplyUserInterceptor>(),
                    serviceProvider.GetRequiredService<ApplyAuditInfoInterceptor>()
                )
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDbContextFactory<AppDbContext>, CustomDbContextFactory>();

        return services;
    }

    public static string GetDefaultConnectionString(IConfiguration configurations)
    {
        return configurations.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
}