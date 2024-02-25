using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Categories;

internal class DeleteCategoryCommandHandler(
    INotificationManager notificationManager,
    ICategoryRepository categoryRepository) : ICommandHandler<DeleteCategoryCommand>
{
    public async ValueTask<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            AddCategoryNotFoundNotification();
            return default;
        }

        categoryRepository.Delete(category);

        return default;
    }

    private void AddCategoryNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.CategoryNotFound);
    }
}