namespace ProjectK.Core.Infrastructure.Notifications;

public record Notification(
    NotificationType Type,
    string Message
);