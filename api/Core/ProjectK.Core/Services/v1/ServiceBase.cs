using ProjectK.Core.Infrastructure.Notifications.Infrastructure;

namespace ProjectK.Core.Services.v1;

public abstract class ServiceBase(INotificationManager notificationManager)
{
    protected readonly INotificationManager NotificationManager = notificationManager;
}