using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Infrastructure.Notifications;
using ProjectK.Infrastructure.RequestContext;

namespace ProjectK.Infrastructure;

[ExcludeFromCodeCoverage]
public static class ProjectDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddScoped<INotificationManager, NotificationManager>();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}