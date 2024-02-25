using Mediator;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Commands.v1.PlannedTransactions.Delete;
using ProjectK.Core.Enums;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions;

internal class DeletePlannedTransactionUseCase(
    INotificationManager notificationManager,
    ISender mediator) : IDeletePlannedTransactionUseCase
{
    public async Task ExecuteAsync(Ulid plannedTransactionId,
        DeletePlannedTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        DeletePlannedTransactionCommandBase? command = request.ActionType switch
        {
            ActionType.All => new DeletePlannedTransactionCommand(plannedTransactionId),
            ActionType.JustOne => new DeleteMonthlyPlannedTransactionCommand(plannedTransactionId, request.Year,
                request.Month),
            ActionType.FromNowOn => new DeleteFromNowOnPlannedTransaction(plannedTransactionId, request.Year,
                request.Month),
            _ => null
        };

        if (command is null)
        {
            AddInvalidActionTypeNotification();
            return;
        }

        await mediator.Send(command, cancellationToken);
    }

    private void AddInvalidActionTypeNotification()
    {
        notificationManager.Add(NotificationType.BadRequest, Resources.InvalidActionTypeProvided);
    }
}