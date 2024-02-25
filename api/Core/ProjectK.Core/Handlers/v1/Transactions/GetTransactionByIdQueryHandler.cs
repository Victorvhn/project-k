using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Queries.v1.Transactions;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Transactions;

internal sealed class GetTransactionByIdQueryHandler(
    INotificationManager notificationManager,
    ITransactionRepository transactionRepository) : IQueryHandler<GetTransactionByIdQuery, Transaction?>
{
    public async ValueTask<Transaction?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

        if (transaction is not null) return transaction;

        AddTransactionNotFoundNotification();
        return default;
    }

    private void AddTransactionNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.TransactionNotFound);
    }
}