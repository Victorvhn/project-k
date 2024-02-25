using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ProjectK.Api.Attributes;
using ProjectK.Core.Adapters.Driven.Database;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Database.Contexts;

namespace ProjectK.Api.Filters;

public class TransactionFilter(
    ILogger<TransactionFilter> logger,
    AppDbContext appDbContext,
    IUnitOfWork unitOfWork,
    INotificationManager notificationManager)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasTransactionAttribute = context
            .ActionDescriptor
            .EndpointMetadata
            .Any(a => a.GetType() == typeof(TransactionAttribute));

        if (!hasTransactionAttribute)
        {
            await next();
            return;
        }

        var strategy = appDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await unitOfWork.BeginTransactionAsync();

            var result = await next();

            if (result.Exception is not null && result.ExceptionHandled is false)
            {
                await unitOfWork.RollbackAsync(context.HttpContext.RequestAborted);
                return;
            }

            if (notificationManager.HasNotifications)
            {
                logger.LogWarning(
                    "Transaction will be rolled back due to notifications being present. Found {NotificationCount} notifications with the following messages: {NotificationMessages}",
                    notificationManager.Notifications.Count,
                    JsonSerializer.Serialize(notificationManager.Notifications));

                await unitOfWork.RollbackAsync(context.HttpContext.RequestAborted);

                return;
            }

            await unitOfWork.CommitAsync();
        });
    }
}