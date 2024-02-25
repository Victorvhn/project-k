using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.PlannedTransactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Core.Services.v1.Interfaces;

namespace ProjectK.Core.Handlers.v1.PlannedTransactions;

internal sealed class CreatePlannedTransactionCommandHandler(
    INotificationManager notificationManager,
    IPlannedTransactionRepository plannedTransactionRepository,
    ICategoryService categoryService) : ICommandHandler<CreatePlannedTransactionCommand, PlannedTransaction?>
{
    public async ValueTask<PlannedTransaction?> Handle(CreatePlannedTransactionCommand request,
        CancellationToken cancellationToken)
    {
        if (!await IsCategoryValidAsync(request.CategoryId, cancellationToken))
        {
            AddCategoryNotFoundNotification();
            return default;
        }

        var plannedTransaction = PlannedTransaction.CreateInstance(request);

        await plannedTransactionRepository.AddAsync(plannedTransaction, cancellationToken);

        return plannedTransaction;
    }

    private async ValueTask<bool> IsCategoryValidAsync(Ulid? categoryId, CancellationToken cancellationToken)
    {
        if (categoryId is null)
            return true;

        var categoryExists =
            await categoryService.ExistsByIdAsync(categoryId.Value, cancellationToken);

        return categoryExists;
    }

    private void AddCategoryNotFoundNotification()
    {
        notificationManager.Add(NotificationType.BadRequest, Resources.CategoryNotFound);
    }
}