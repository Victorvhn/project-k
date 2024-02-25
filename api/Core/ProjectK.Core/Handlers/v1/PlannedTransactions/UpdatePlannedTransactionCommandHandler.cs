using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.PlannedTransactions.Update;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Core.Services.v1.Interfaces;

namespace ProjectK.Core.Handlers.v1.PlannedTransactions;

internal sealed class UpdatePlannedTransactionCommandHandler(
    INotificationManager notificationManager,
    IPlannedTransactionRepository plannedTransactionRepository,
    ICategoryService categoryService) : ICommandHandler<UpdatePlannedTransactionCommand, PlannedTransactionDto?>,
    ICommandHandler<UpdateMonthlyPlannedTransactionCommand, PlannedTransactionDto?>,
    ICommandHandler<UpdateFromNowOnPlannedTransactionCommand, PlannedTransactionDto?>
{
    public async ValueTask<PlannedTransactionDto?> Handle(UpdateFromNowOnPlannedTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var filter = new MonthlyFilter(request.Year, request.Month);

        var plannedTransaction =
            await plannedTransactionRepository.GetByIdWithAllCustomPlannedBeforeDateAsync(request.PlannedTransactionId,
                filter, cancellationToken);

        if (plannedTransaction is null)
        {
            AddPlannedTransactionNotFoundNotification();
            return default;
        }

        var datesDifference =
            new DateTime(request.Year, request.Month, plannedTransaction.StartsAt.Day, 0, 0, 0, DateTimeKind.Utc) -
            plannedTransaction.StartsAt.ToDateTime(new TimeOnly());

        var differenceByRecurrence = (int)Math.Round(plannedTransaction.Recurrence switch
        {
            Recurrence.Annual => datesDifference.TotalDays / 365.25,
            Recurrence.Monthly => datesDifference.TotalDays / 30.4375,
            _ => 0
        });

        for (var i = 1; i <= differenceByRecurrence; i++)
        {
            var refersTo = plannedTransaction.Recurrence switch
            {
                Recurrence.Annual => new DateOnly(request.Year, request.Month,
                    DayOnlySafe.Get(filter, plannedTransaction.StartsAt.Day)).AddYears(-i),
                Recurrence.Monthly => new DateOnly(request.Year, request.Month,
                    DayOnlySafe.Get(filter, plannedTransaction.StartsAt.Day)).AddMonths(-i),
                _ => default
            };

            if (plannedTransaction.CustomPlannedTransactions.Any(a => a.RefersTo.Year == refersTo.Year && a.RefersTo.Month == refersTo.Month))
                continue;

            var customPlannedTransaction =
                CustomPlannedTransaction.CreateInstance(plannedTransaction, refersTo);

            plannedTransaction.AddCustomPlannedTransaction(customPlannedTransaction);
        }

        plannedTransaction.Update(request, filter);

        plannedTransactionRepository.Update(plannedTransaction);

        return PlannedTransactionDto.CreateInstance(plannedTransaction, plannedTransaction.CustomPlannedTransactions.Count != 0);
    }

    public async ValueTask<PlannedTransactionDto?> Handle(UpdateMonthlyPlannedTransactionCommand request,
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
            var refersTo = new DateOnly(request.Year, request.Month, DayOnlySafe.Get(filter, request.StartsAt.Day));

            var newCustomPlannedTransaction =
                CustomPlannedTransaction.CreateInstance(plannedTransaction, request, refersTo);

            plannedTransaction.AddCustomPlannedTransaction(newCustomPlannedTransaction);
        }
        else
        {
            plannedTransaction.CustomPlannedTransactions.First().Update(request);
        }

        plannedTransactionRepository.Update(plannedTransaction);

        return PlannedTransactionDto.CreateInstance(plannedTransaction,
            plannedTransaction.CustomPlannedTransactions.First());
    }

    public async ValueTask<PlannedTransactionDto?> Handle(UpdatePlannedTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var plannedTransaction =
            await plannedTransactionRepository.GetByIdWithCustomPlannedAsync(
                request.PlannedTransactionId, cancellationToken);

        if (plannedTransaction is null)
        {
            AddPlannedTransactionNotFoundNotification();
            return default;
        }

        if (!await IsCategoryValidAsync(request.CategoryId, cancellationToken)) return default;

        if (!CanUpdateStartDate(plannedTransaction, request.StartsAt))
        {
            AddCanNotUpdateStartsAtWhenThereAreCustomPlannedTransactionsNotification();
            return default;
        }

        foreach (var customPlannedTransaction in plannedTransaction.CustomPlannedTransactions)
            customPlannedTransaction.Update(request);

        plannedTransaction.Update(request);

        plannedTransactionRepository.Update(plannedTransaction);

        return PlannedTransactionDto.CreateInstance(plannedTransaction, plannedTransaction.CustomPlannedTransactions.Count != 0);
    }

    private async ValueTask<bool> IsCategoryValidAsync(Ulid? categoryId, CancellationToken cancellationToken)
    {
        if (categoryId is null)
            return true;

        var categoryExists =
            await categoryService.ExistsByIdAsync(categoryId.Value, cancellationToken);

        if (!categoryExists) AddCategoryNotFoundNotification();

        return categoryExists;
    }

    private static bool CanUpdateStartDate(PlannedTransaction plannedTransaction, DateOnly startDate)
    {
        if (!plannedTransaction.CustomPlannedTransactions.Any())
        {
            return true;
        }
        
        return plannedTransaction.StartsAt.Year == startDate.Year &&
               plannedTransaction.StartsAt.Month == startDate.Month;
    }

    private void AddPlannedTransactionNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }

    private void AddCategoryNotFoundNotification()
    {
        notificationManager.Add(NotificationType.BadRequest, Resources.CategoryNotFound);
    }

    private void AddCanNotUpdateStartsAtWhenThereAreCustomPlannedTransactionsNotification()
    {
        notificationManager.Add(NotificationType.BadRequest, Resources.CanNotUpdateStartsAt);
    }
}