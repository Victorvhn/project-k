namespace ProjectK.Core.Infrastructure.Notifications.Infrastructure;

public interface INotificationManager
{
    public bool HasNotifications { get; }
    public IReadOnlyList<Notification> Notifications { get; }

    public void Add(Notification notification);
    public void Add(NotificationType type, string message);
}