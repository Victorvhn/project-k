using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Services.v1.Interfaces;

namespace ProjectK.Core.Services.v1;

internal class CategoryService(
    INotificationManager notificationManager,
    ICategoryRepository categoryRepository) : ServiceBase(notificationManager), ICategoryService
{
    public async Task<bool> ExistsByIdAsync(Ulid categoryId, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.ExistsByIdAsync(categoryId, cancellationToken);
    }
}