using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using ProjectK.Core.Adapters.Driven.Database;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Database.Interceptors;
using ProjectK.Database.Repositories.v1;

namespace ProjectK.Database;

[ExcludeFromCodeCoverage]
public static class ProjectDependencies
{
    public static IServiceCollection AddDatabaseDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPlannedTransactionRepository, PlannedTransactionRepository>();
        services.AddScoped<ICustomPlannedTransactionRepository, CustomPlannedTransactionRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddSingleton<QueryLimitCommandInterceptor>();
        services.AddScoped<ApplyUserInterceptor>();
        services.AddScoped<ApplyAuditInfoInterceptor>();

        return services;
    }
}