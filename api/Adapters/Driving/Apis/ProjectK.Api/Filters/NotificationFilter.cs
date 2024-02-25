using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;

namespace ProjectK.Api.Filters;

public class NotificationFilter(
    INotificationManager notificationManager,
    ProblemDetailsFactory problemDetailsFactory,
    ILogger<NotificationFilter> logger)
    : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (!notificationManager.HasNotifications)
        {
            await next();
            return;
        }

        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            context.HttpContext,
            title: GetTitle(),
            detail: GetDetail(),
            statusCode: GetStatusCode(),
            instance: context.HttpContext.Request.Path
        );

        logger.LogWarning(
            "Notification received: '{ActionName}' triggered on '{ControllerName}' with details: '{ProblemDetailsDetail}'",
            context.ActionDescriptor.DisplayName, context.ActionDescriptor.RouteValues["controller"],
            problemDetails.Detail);

        context.Result = new ObjectResult(problemDetails);

        await next();
    }

    private int GetStatusCode()
    {
        int statusCode;
        if (AreAllNotificationsTheSameType())
            statusCode = notificationManager.Notifications[0].Type switch
            {
                NotificationType.Conflict => 409,
                NotificationType.BusinessRule => 422,
                NotificationType.BadRequest => 400,
                NotificationType.NotFound => 404,
                _ => 400
            };
        else
            statusCode = 400;

        return statusCode;
    }

    private string GetTitle()
    {
        string title;
        if (IsThereMoreThanOneNotification())
        {
            if (AreAllNotificationsTheSameType())
                title = notificationManager.Notifications[0].Type switch
                {
                    NotificationType.Conflict => "Multiple conflicts occurred.",
                    NotificationType.BusinessRule => "Multiple business rules were violated.",
                    NotificationType.BadRequest => "Multiple invalid sources were not provided.",
                    NotificationType.NotFound => "Multiple resources were not found.",
                    _ => "Multiple errors occurred."
                };
            else
                title = "Multiple errors occurred.";
        }
        else
        {
            title = notificationManager.Notifications[0].Type switch
            {
                NotificationType.Conflict => "Conflict occurred.",
                NotificationType.BusinessRule => "Business rule violation.",
                NotificationType.BadRequest => "Invalid source provided.",
                NotificationType.NotFound => "Resource not found.",
                _ => "Error occurred."
            };
        }

        return title;
    }

    private string GetDetail()
    {
        var detail = IsThereMoreThanOneNotification()
            ? string.Join("\n", notificationManager.Notifications.Select(n => n.Message))
            : notificationManager.Notifications[0].Message;

        return detail;
    }

    private bool AreAllNotificationsTheSameType()
    {
        return notificationManager.Notifications.DistinctBy(d => d.Type).Count() == 1;
    }

    private bool IsThereMoreThanOneNotification()
    {
        return notificationManager.Notifications.Count > 1;
    }
}