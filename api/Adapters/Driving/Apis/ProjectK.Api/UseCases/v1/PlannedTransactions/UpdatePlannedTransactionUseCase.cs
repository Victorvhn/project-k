using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Commands.v1.PlannedTransactions.Update;
using ProjectK.Core.Enums;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using PlannedTransactionDto = ProjectK.Api.Dtos.v1.PlannedTransactions.Responses.PlannedTransactionDto;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions;

internal class UpdatePlannedTransactionUseCase(
    IMapper mapper,
    INotificationManager notificationManager,
    ISender mediator) : IUpdatePlannedTransactionUseCase
{
    public async Task<PlannedTransactionDto?> ExecuteAsync(Ulid plannedTransactionId,
        UpdatePlannedTransactionRequest request, CancellationToken cancellationToken = default)
    {
        UpdatePlannedTransactionCommandBase? command = request.ActionType switch
        {
            ActionType.All => new UpdatePlannedTransactionCommand(plannedTransactionId, request.Description,
                request.Amount, request.AmountType, request.Type, request.Recurrence,
                request.StartsAt,
                request.EndsAt, request.CategoryId),
            ActionType.JustOne => new UpdateMonthlyPlannedTransactionCommand(plannedTransactionId, request.Description,
                request.Amount, request.StartsAt, request.Year, request.Month),
            ActionType.FromNowOn => new UpdateFromNowOnPlannedTransactionCommand(plannedTransactionId, request.Year,
                request.Month, request.Description, request.Amount, request.AmountType, request.Type,
                request.Recurrence, request.StartsAt, request.EndsAt, request.CategoryId),
            _ => null
        };

        if (command is null)
        {
            AddInvalidActionTypeNotification();
            return default;
        }

        var result = await mediator.Send(command, cancellationToken);

        return mapper.Map<PlannedTransactionDto?>(result);
    }

    private void AddInvalidActionTypeNotification()
    {
        notificationManager.Add(NotificationType.BadRequest, Resources.InvalidActionTypeProvided);
    }
}