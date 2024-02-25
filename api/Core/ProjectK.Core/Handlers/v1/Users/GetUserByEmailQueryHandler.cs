using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Queries.v1.Users;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Users;

internal sealed class GetUserByEmailQueryHandler(
    INotificationManager notificationManager,
    IUserRepository userRepository) : IQueryHandler<GetUserByEmailQuery, User?>
{
    public async ValueTask<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is not null) return user;

        AddUserNotFoundNotification(request.Email);
        return default;
    }

    private void AddUserNotFoundNotification(string email)
    {
        notificationManager.Add(NotificationType.NotFound,
            string.Format(Resources.UserWithEmailNotFound, email));
    }
}