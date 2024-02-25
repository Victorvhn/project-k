using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Core.Services.v1.Interfaces;

namespace ProjectK.Core.Handlers.v1.Transactions;

internal sealed class UpdateTransactionCommandHandler(
    INotificationManager notificationManager,
    ITransactionRepository transactionRepository,
    ICategoryService categoryService,
    IPlannedTransactionService plannedTransactionService) : ICommandHandler<UpdateTransactionCommand, Transaction?>
{
    public async ValueTask<Transaction?> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

        if (transaction is null)
        {
            AddTransactionNotFoundNotification();
            return default;
        }

        if (!await IsCategoryValidAsync(request.CategoryId, cancellationToken) ||
            !await IsPlannedTransactionValidAsync(request.PlannedTransactionId, cancellationToken))
            return default;

        transaction.Update(request);

        transactionRepository.Update(transaction);

        return transaction;
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

    private async Task<bool> IsPlannedTransactionValidAsync(Ulid? plannedTransactionId,
        CancellationToken cancellationToken)
    {
        if (plannedTransactionId is null)
            return true;

        var transactionExists =
            await plannedTransactionService.ExistsByIdAsync(plannedTransactionId.Value, cancellationToken);

        if (!transactionExists) AddPlannedTransactionNotFoundNotification();

        return transactionExists;
    }

    private void AddTransactionNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.TransactionNotFound);
    }

    private void AddCategoryNotFoundNotification()
    {
        notificationManager.Add(NotificationType.BadRequest, Resources.CategoryNotFound);
    }

    private void AddPlannedTransactionNotFoundNotification()
    {
        notificationManager.Add(NotificationType.BadRequest, Resources.PlannedTransactionNotFound);
    }
}