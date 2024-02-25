using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Queries.v1.PlannedTransactions;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.PlannedTransactions;

internal sealed class GetPlannedTransactionByIdAndDateQueryHandler(
    INotificationManager notificationManager,
    IPlannedTransactionRepository plannedTransactionRepository,
    ICustomPlannedTransactionRepository customPlannedTransactionRepository)
    : IQueryHandler<GetPlannedTransactionByIdAndDateQuery, PlannedTransactionDto?>
{
    public async ValueTask<PlannedTransactionDto?> Handle(GetPlannedTransactionByIdAndDateQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new MonthlyFilter(request.Year, request.Month);

        var plannedTransaction =
            await plannedTransactionRepository.GetByIdAndDateWithCustomPlannedAsync(request.PlannedTransactionId,
                filter,
                cancellationToken);

        var isThereAnyCustomPlannedTransaction =
            await customPlannedTransactionRepository.AnyByPlannedTransactionIdAsync(request.PlannedTransactionId);

        if (plannedTransaction is null)
        {
            AddPlannedTransactionNotFoundNotification();
            return default;
        }

        return plannedTransaction.CustomPlannedTransactions.Count == 0
            ? PlannedTransactionDto.CreateInstance(plannedTransaction, isThereAnyCustomPlannedTransaction)
            : PlannedTransactionDto.CreateInstance(plannedTransaction,
                plannedTransaction.CustomPlannedTransactions.First());
    }

    private void AddPlannedTransactionNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }
}