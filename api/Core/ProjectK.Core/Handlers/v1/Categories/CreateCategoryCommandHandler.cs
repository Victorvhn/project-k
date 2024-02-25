using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Categories;

internal sealed class CreateCategoryCommandHandler(
    INotificationManager notificationManager,
    ICategoryRepository categoryRepository) : ICommandHandler<CreateCategoryCommand, Category?>
{
    public async ValueTask<Category?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await categoryRepository.ExistsByNameAsync(request.Name, cancellationToken);

        if (categoryExists)
        {
            AddCategoryAlreadyExistsNotification(request.Name);
            return default;
        }

        var category = Category.CreateInstance(request);

        await categoryRepository.AddAsync(category, cancellationToken);

        return category;
    }

    private void AddCategoryAlreadyExistsNotification(string name)
    {
        notificationManager.Add(NotificationType.Conflict, string.Format(Resources.CategoryAlreadyExists, name));
    }
}