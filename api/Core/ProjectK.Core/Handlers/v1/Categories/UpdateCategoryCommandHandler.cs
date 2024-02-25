using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Categories;

internal class UpdateCategoryCommandHandler(
    INotificationManager notificationManager,
    ICategoryRepository categoryRepository) : ICommandHandler<UpdateCategoryCommand, Category?>
{
    public async ValueTask<Category?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            AddCategoryNameNotFoundNotification(request.Name);
            return default;
        }

        category.Update(request);

        categoryRepository.Update(category);

        return category;
    }

    private void AddCategoryNameNotFoundNotification(string name)
    {
        notificationManager.Add(NotificationType.NotFound,
            string.Format(Resources.CategoryNameNotFound, name));
    }
}