using System.Diagnostics.CodeAnalysis;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using ProjectK.Core.Behaviours;
using ProjectK.Core.Services.v1;
using ProjectK.Core.Services.v1.Interfaces;

namespace ProjectK.Core;

[ExcludeFromCodeCoverage]
public static class ProjectDependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPlannedTransactionService, PlannedTransactionService>();

        return services;
    }

    public static IServiceCollection AddMediatorConfig(this IServiceCollection services)
    {
        services.AddMediator(cfg => { cfg.ServiceLifetime = ServiceLifetime.Scoped; });
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehaviour<,>));

        return services;
    }
}