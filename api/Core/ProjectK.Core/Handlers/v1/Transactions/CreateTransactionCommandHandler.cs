using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Core.Services.v1.Interfaces;

namespace ProjectK.Core.Handlers.v1.Transactions;

internal sealed class CreateTransactionCommandHandler(
    INotificationManager notificationManager,
    ITransactionRepository transactionRepository,
    ICategoryService categoryService,
    IPlannedTransactionService plannedTransactionService) : ICommandHandler<CreateTransactionCommand, Transaction?>
{
    public async ValueTask<Transaction?> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        if (!await IsCategoryValidAsync(request.CategoryId, cancellationToken))
        {
            AddCategoryNotFoundNotification();
            return default;
        }

        if (!await IsPlannedTransactionValidAsync(request.PlannedTransactionId, cancellationToken))
        {
            AddPlannedTransactionNotFoundNotification();
            return default;
        }

        var transaction = Transaction.CreateInstance(request);

        await transactionRepository.AddAsync(transaction, cancellationToken);

        return transaction;
    }

    private async ValueTask<bool> IsCategoryValidAsync(Ulid? categoryId, CancellationToken cancellationToken)
    {
        if (categoryId is null)
            return true;

        var categoryExists =
            await categoryService.ExistsByIdAsync(categoryId.Value, cancellationToken);

        return categoryExists;
    }

    private async Task<bool> IsPlannedTransactionValidAsync(Ulid? dataPlannedTransactionId,
        CancellationToken cancellationToken)
    {
        if (dataPlannedTransactionId is null)
            return true;

        var transactionExists =
            await plannedTransactionService.ExistsByIdAsync(dataPlannedTransactionId.Value, cancellationToken);

        return transactionExists;
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