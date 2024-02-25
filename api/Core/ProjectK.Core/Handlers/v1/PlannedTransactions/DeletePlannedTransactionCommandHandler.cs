using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.PlannedTransactions.Delete;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.PlannedTransactions;

internal sealed class DeletePlannedTransactionCommandHandler(
    INotificationManager notificationManager,
    IPlannedTransactionRepository plannedTransactionRepository) : ICommandHandler<DeletePlannedTransactionCommand>,
    ICommandHandler<DeleteMonthlyPlannedTransactionCommand>,
    ICommandHandler<DeleteFromNowOnPlannedTransaction>
{
    public async ValueTask<Unit> Handle(DeleteFromNowOnPlannedTransaction request, CancellationToken cancellationToken)
    {
        var filter = new MonthlyFilter(request.Year, request.Month);

        var plannedTransaction =
            await plannedTransactionRepository.GetByIdAndDateWithCustomPlannedAsync(request.PlannedTransactionId,
                filter, cancellationToken);

        if (plannedTransaction is null)
        {
            AddPlannedTransactionNotFoundNotification();
            return default;
        }

        if (plannedTransaction.CustomPlannedTransactions.Count > 0)
        {
            var customPlannedTransaction = plannedTransaction.CustomPlannedTransactions.First();

            customPlannedTransaction.Inactivate();
        }

        plannedTransaction.Inactivate(filter);

        plannedTransactionRepository.Update(plannedTransaction);

        return default;
    }

    public async ValueTask<Unit> Handle(DeleteMonthlyPlannedTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var filter = new MonthlyFilter(request.Year, request.Month);

        var plannedTransaction =
            await plannedTransactionRepository.GetByIdAndDateWithCustomPlannedAsync(request.PlannedTransactionId,
                filter, cancellationToken);

        if (plannedTransaction is null)
        {
            AddPlannedTransactionNotFoundNotification();
            return default;
        }

        if (plannedTransaction.CustomPlannedTransactions.Count == 0)
        {
            var isDayAvailable = plannedTransaction.StartsAt.Day <= DateTime.DaysInMonth(request.Year, request.Month);

            var refersTo = new DateOnly(request.Year, request.Month,
                isDayAvailable ? plannedTransaction.StartsAt.Day : DateTime.DaysInMonth(request.Year, request.Month));

            var customPlannedTransaction =
                CustomPlannedTransaction.CreateInactiveInstance(plannedTransaction, refersTo);

            plannedTransaction.AddCustomPlannedTransaction(customPlannedTransaction);

            plannedTransactionRepository.Update(plannedTransaction);
        }
        else
        {
            var customPlannedTransaction = plannedTransaction.CustomPlannedTransactions.First();

            customPlannedTransaction.Inactivate();

            plannedTransactionRepository.Update(plannedTransaction);
        }

        return default;
    }

    public async ValueTask<Unit> Handle(DeletePlannedTransactionCommand request, CancellationToken cancellationToken)
    {
        var plannedTransaction =
            await plannedTransactionRepository.GetByIdAsync(request.PlannedTransactionId, cancellationToken);

        if (plannedTransaction is null)
        {
            AddPlannedTransactionNotFoundNotification();
            return default;
        }

        plannedTransactionRepository.Delete(plannedTransaction);

        return default;
    }

    private void AddPlannedTransactionNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }
}