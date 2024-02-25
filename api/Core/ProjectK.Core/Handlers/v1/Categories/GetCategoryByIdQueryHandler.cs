using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Queries.v1.Categories;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Categories;

internal class GetCategoryByIdQueryHandler(
    INotificationManager notificationManager,
    ICategoryRepository categoryRepository) : IQueryHandler<GetCategoryByIdQuery, Category?>
{
    public async ValueTask<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            AddCategoryNotFoundNotification();
            return default;
        }

        return category;
    }

    private void AddCategoryNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.CategoryNotFound);
    }
}