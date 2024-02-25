using AutoFixture;
using FluentAssertions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Infrastructure.Notifications;
using ProjectK.Tests.Shared;

namespace ProjectK.Infrastructure.Tests.Notifications;

public class NotificationManagerTests
{
    private readonly Fixture _fixture = CustomAutoFixture.Create();

    private readonly INotificationManager _notificationManager;

    public NotificationManagerTests()
    {
        _notificationManager = new NotificationManager();
    }

    [Fact]
    public void It_should_add_notification_passing_object()
    {
        // Arrange
        var notification = _fixture.Create<Notification>();

        // Act
        var act = () => _notificationManager.Add(notification);

        // Assert
        act
            .Should()
            .NotThrow<Exception>();
    }

    [Fact]
    public void It_should_add_notification_passing_params()
    {
        // Act
        var act = () => _notificationManager.Add(NotificationType.BusinessRule, "message");

        // Assert
        act
            .Should()
            .NotThrow<Exception>();
    }

    [Fact]
    public void It_should_verify_has_notifications()
    {
        // Arrange
        var notification = _fixture.Create<Notification>();

        // Act
        var resultBefore = _notificationManager.HasNotifications;

        _notificationManager.Add(notification);

        var resultAfter = _notificationManager.HasNotifications;

        // Assert
        resultBefore
            .Should()
            .BeFalse();
        resultAfter
            .Should()
            .BeTrue();
    }

    [Fact]
    public void It_should_get_notifications()
    {
        // Arrange
        var notification = _fixture.Create<Notification>();

        _notificationManager.Add(notification);

        // Act
        var result = _notificationManager.Notifications;

        // Assert
        result
            .Should()
            .AllBeEquivalentTo(notification);
    }
}