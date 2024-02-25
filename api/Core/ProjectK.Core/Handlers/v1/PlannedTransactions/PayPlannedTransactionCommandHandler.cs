using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.PlannedTransactions;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.PlannedTransactions;

internal sealed class PayPlannedTransactionCommandHandler(
    INotificationManager notificationManager,
    ITransactionRepository transactionRepository,
    IPlannedTransactionRepository plannedTransactionRepository) : ICommandHandler<PayPlannedTransactionCommand>
{
    public async ValueTask<Unit> Handle(PayPlannedTransactionCommand request, CancellationToken cancellationToken)
    {
        var monthlyFilter = new MonthlyFilter(request.Year, request.Month);

        var plannedTransaction =
            await plannedTransactionRepository.GetByIdAndDateWithCustomPlannedAsync(request.PlannedTransactionId,
                monthlyFilter, cancellationToken);

        if (plannedTransaction is null)
        {
            AddUnableToCreateTransactionNotification();
            return default;
        }

        var transaction = Transaction.CreatePayment(plannedTransaction, monthlyFilter, request.Amount);

        await transactionRepository.AddAsync(transaction, cancellationToken);

        return default;
    }

    private void AddUnableToCreateTransactionNotification()
    {
        notificationManager.Add(NotificationType.BusinessRule, Resources.UnableToCreateTransaction);
    }
}