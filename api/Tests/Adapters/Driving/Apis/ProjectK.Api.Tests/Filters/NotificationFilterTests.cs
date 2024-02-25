using System.Reflection;
using AutoFixture;
using FluentAssertions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using ProjectK.Api.Filters;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Tests.Shared;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace ProjectK.Api.Tests.Filters;

public class NotificationFilterTests
{
    private static readonly Fixture _fixture = CustomAutoFixture.Create();
    private readonly ILogger<NotificationFilter> _logger;

    private readonly NotificationFilter _notificationFilter;
    private readonly INotificationManager _notificationManager;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ProblemDetails defaultProblemDetails = _fixture.Create<ProblemDetails>();

    public NotificationFilterTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _logger = Substitute.For<ILogger<NotificationFilter>>();
        var problemDetailsOptions = new ProblemDetailsOptions
        {
            IncludeExceptionDetails = (_, _) => false,
            MapStatusCode = _ => defaultProblemDetails
        };
        _problemDetailsFactory = new ProblemDetailsFactory(Options.Create(problemDetailsOptions),
            Substitute.For<ILogger<ProblemDetailsFactory>>(), Substitute.For<IHostEnvironment>());

        // For some reason, it is not possible to mock ProblemDetailsFactory, so we have to pass null here.
        // All tests is this class are workarounds for this issue.
        _notificationFilter = new NotificationFilter(_notificationManager, _problemDetailsFactory, _logger);
    }

    [Theory]
    [InlineData(NotificationType.Conflict, 409)]
    [InlineData(NotificationType.BusinessRule, 422)]
    [InlineData(NotificationType.BadRequest, 400)]
    [InlineData(NotificationType.NotFound, 404)]
    public void GetStatusCode_should_work_when_all_notifications_are_the_same_type(NotificationType notificationType,
        int expectedResult)
    {
        // Arrange
        var methodInfo =
            typeof(NotificationFilter).GetMethod("GetStatusCode", BindingFlags.NonPublic | BindingFlags.Instance);

        var notification1 = _fixture.Build<Notification>()
            .With(x => x.Type, notificationType)
            .Create();

        var notification2 = _fixture.Build<Notification>()
            .With(x => x.Type, notificationType)
            .Create();

        _notificationManager
            .Notifications
            .Returns(new List<Notification> { notification1, notification2 });

        // Act
        var result = methodInfo?.Invoke(_notificationFilter, null);

        // Assert
        result
            .Should()
            .BeOfType<int>();
        result
            .Should()
            .Be(expectedResult);
    }

    [Fact]
    public void GetStatusCode_should_work_when_notifications_are_not_same_type()
    {
        // Arrange
        var methodInfo =
            typeof(NotificationFilter).GetMethod("GetStatusCode", BindingFlags.NonPublic | BindingFlags.Instance);

        var notification1 = _fixture.Build<Notification>()
            .With(x => x.Type, NotificationType.BusinessRule)
            .Create();

        var notification2 = _fixture.Build<Notification>()
            .With(x => x.Type, NotificationType.BadRequest)
            .Create();

        var notification3 = _fixture.Build<Notification>()
            .With(x => x.Type, NotificationType.NotFound)
            .Create();

        _notificationManager
            .Notifications
            .Returns(new List<Notification> { notification1, notification2, notification3 });

        // Act
        var result = methodInfo?.Invoke(_notificationFilter, null);

        // Assert
        result
            .Should()
            .BeOfType<int>();
        result
            .Should()
            .Be(400);
    }

    [Fact]
    public void GetStatusCode_should_work_when_notification_type_is_invalid()
    {
        // Arrange
        var methodInfo =
            typeof(NotificationFilter).GetMethod("GetStatusCode", BindingFlags.NonPublic | BindingFlags.Instance);

        var notification1 = _fixture.Build<Notification>()
            .With(x => x.Type, (NotificationType)100)
            .Create();

        _notificationManager
            .Notifications
            .Returns(new List<Notification> { notification1 });

        // Act
        var result = methodInfo?.Invoke(_notificationFilter, null);

        // Assert
        result
            .Should()
            .BeOfType<int>();
        result
            .Should()
            .Be(400);
    }

    [Theory]
    [InlineData(NotificationType.Conflict, "Multiple conflicts occurred.")]
    [InlineData(NotificationType.BusinessRule, "Multiple business rules were violated.")]
    [InlineData(NotificationType.BadRequest, "Multiple invalid sources were not provided.")]
    [InlineData(NotificationType.NotFound, "Multiple resources were not found.")]
    [InlineData((NotificationType)100, "Multiple errors occurred.")]
    public void GetTitle_should_work_when_all_notifications_are_the_same_type(NotificationType notificationType,
        string expectedResult)
    {
        // Arrange
        var methodInfo =
            typeof(NotificationFilter).GetMethod("GetTitle", BindingFlags.NonPublic | BindingFlags.Instance);

        var notification1 = _fixture.Build<Notification>()
            .With(x => x.Type, notificationType)
            .Create();

        var notification2 = _fixture.Build<Notification>()
            .With(x => x.Type, notificationType)
            .Create();

        _notificationManager
            .Notifications
            .Returns(new List<Notification> { notification1, notification2 });

        // Act
        var result = methodInfo?.Invoke(_notificationFilter, null);

        // Assert
        result
            .Should()
            .BeOfType<string>();
        result
            .Should()
            .Be(expectedResult);
    }

    [Fact]
    public void GetTitle_should_work_when_notifications_are_not_same_type()
    {
        // Arrange
        var methodInfo =
            typeof(NotificationFilter).GetMethod("GetTitle", BindingFlags.NonPublic | BindingFlags.Instance);

        var notification1 = _fixture.Build<Notification>()
            .With(x => x.Type, NotificationType.BusinessRule)
            .Create();

        var notification2 = _fixture.Build<Notification>()
            .With(x => x.Type, NotificationType.BadRequest)
            .Create();

        var notification3 = _fixture.Build<Notification>()
            .With(x => x.Type, NotificationType.NotFound)
            .Create();

        _notificationManager
            .Notifications
            .Returns(new List<Notification> { notification1, notification2, notification3 });

        // Act
        var result = methodInfo?.Invoke(_notificationFilter, null);

        // Assert
        result
            .Should()
            .BeOfType<string>();
        result
            .Should()
            .Be("Multiple errors occurred.");
    }

    [Theory]
    [InlineData(NotificationType.Conflict, "Conflict occurred.")]
    [InlineData(NotificationType.BusinessRule, "Business rule violation.")]
    [InlineData(NotificationType.BadRequest, "Invalid source provided.")]
    [InlineData(NotificationType.NotFound, "Resource not found.")]
    [InlineData((NotificationType)100, "Error occurred.")]
    public void GetTitle_should_work_when_there_is_only_one_notification(NotificationType notificationType,
        string expectedResult)
    {
        // Arrange
        var methodInfo =
            typeof(NotificationFilter).GetMethod("GetTitle", BindingFlags.NonPublic | BindingFlags.Instance);

        var notification1 = _fixture.Build<Notification>()
            .With(x => x.Type, notificationType)
            .Create();

        _notificationManager
            .Notifications
            .Returns(new List<Notification> { notification1 });

        // Act
        var result = methodInfo?.Invoke(_notificationFilter, null);

        // Assert
        result
            .Should()
            .BeOfType<string>();
        result
            .Should()
            .Be(expectedResult);
    }

    [Fact]
    public void GetDetail_should_work_when_there_is_only_one_notification()
    {
        // Arrange
        var methodInfo =
            typeof(NotificationFilter).GetMethod("GetDetail", BindingFlags.NonPublic | BindingFlags.Instance);

        var notification1 = _fixture.Build<Notification>()
            .With(x => x.Message, "message")
            .Create();

        _notificationManager
            .Notifications
            .Returns(new List<Notification> { notification1 });

        // Act
        var result = methodInfo?.Invoke(_notificationFilter, null);

        // Assert
        result
            .Should()
            .BeOfType<string>();
        result
            .Should()
            .Be("message");
    }

    [Fact]
    public void GetDetail_should_work_when_there_are_multiple_notifications()
    {
        // Arrange
        var methodInfo =
            typeof(NotificationFilter).GetMethod("GetDetail", BindingFlags.NonPublic | BindingFlags.Instance);

        var notification1 = _fixture.Build<Notification>()
            .With(x => x.Message, "message1")
            .Create();

        var notification2 = _fixture.Build<Notification>()
            .With(x => x.Message, "message2")
            .Create();

        _notificationManager
            .Notifications
            .Returns(new List<Notification> { notification1, notification2 });

        // Act
        var result = methodInfo?.Invoke(_notificationFilter, null);

        // Assert
        result
            .Should()
            .BeOfType<string>();
        result
            .Should()
            .Be("message1\nmessage2");
    }

    [Fact]
    public async Task It_should_be_ignored_if_no_notifications_exist()
    {
        // Arrange
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            Substitute.For<RouteData>(),
            new ActionDescriptor(),
            new ModelStateDictionary()
        );

        var resultExecutingContext = new ResultExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new OkObjectResult(null),
            Substitute.For<Controller>()
        );

        _notificationManager.HasNotifications.Returns(false);

        var next = Substitute.For<ResultExecutionDelegate>();

        // Act
        await _notificationFilter.OnResultExecutionAsync(resultExecutingContext, next);

        // Assert
        await next
            .Received(1)();
        resultExecutingContext
            .Result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task It_should_create_problem_details()
    {
        // Arrange
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            Substitute.For<RouteData>(),
            new ActionDescriptor
            {
                DisplayName = "TestMethod",
                RouteValues = new Dictionary<string, string>
                {
                    { "controller", "test" }
                }!
            },
            new ModelStateDictionary()
        );

        var resultExecutingContext = new ResultExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new OkObjectResult(null),
            Substitute.For<Controller>()
        );

        _notificationManager
            .HasNotifications
            .Returns(true);

        _notificationManager
            .Notifications
            .Returns(new List<Notification>
            {
                _fixture.Build<Notification>()
                    .With(w => w.Type, NotificationType.NotFound)
                    .With(w => w.Message, "TestMessage")
                    .Create()
            });

        var next = Substitute.For<ResultExecutionDelegate>();

        // Act
        await _notificationFilter.OnResultExecutionAsync(resultExecutingContext, next);

        // Assert
        await next
            .Received(1)();
        resultExecutingContext
            .Result
            .Should()
            .BeOfType<ObjectResult>();
        resultExecutingContext
            .Result
            .As<ObjectResult>()
            .Value
            .As<ProblemDetails>()
            .Should()
            .BeEquivalentTo(defaultProblemDetails);
    }
}