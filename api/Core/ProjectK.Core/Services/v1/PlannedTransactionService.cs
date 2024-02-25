using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Services.v1.Interfaces;

namespace ProjectK.Core.Services.v1;

internal class PlannedTransactionService(
    INotificationManager notificationManager,
    IPlannedTransactionRepository plannedTransactionRepository)
    : ServiceBase(notificationManager), IPlannedTransactionService
{
    public async Task<bool> ExistsByIdAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        return await plannedTransactionRepository.ExistsByIdAsync(id, cancellationToken);
    }
}