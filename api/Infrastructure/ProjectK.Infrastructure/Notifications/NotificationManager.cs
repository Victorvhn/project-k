using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;

namespace ProjectK.Infrastructure.Notifications;

internal class NotificationManager : INotificationManager
{
    private readonly IList<Notification> _notifications = [];

    public bool HasNotifications => _notifications.Count != 0;
    public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();

    public void Add(Notification notification)
    {
        _notifications.Add(notification);
    }

    public void Add(NotificationType type, string message)
    {
        _notifications.Add(new Notification(type, message));
    }
}