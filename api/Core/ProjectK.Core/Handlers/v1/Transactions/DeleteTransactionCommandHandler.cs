using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Transactions;

internal sealed class DeleteTransactionCommandHandler(
    INotificationManager notificationManager,
    ITransactionRepository transactionRepository) : ICommandHandler<DeleteTransactionCommand>
{
    public async ValueTask<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

        if (transaction is null)
        {
            AddTransactionNotFoundNotification();
            return default;
        }

        transactionRepository.Delete(transaction);

        return default;
    }

    private void AddTransactionNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.TransactionNotFound);
    }
}