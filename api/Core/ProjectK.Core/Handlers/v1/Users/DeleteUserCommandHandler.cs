using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Users;

internal sealed class DeleteUserCommandHandler(
    INotificationManager notificationManager,
    IUserRepository userRepository,
    IUserContext userContext) : ICommandHandler<DeleteUserCommand>
{
    public async ValueTask<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (userContext.UserId != request.UserId)
        {
            AddCanNotProceedNotification();
            return default;
        }

        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            AddUserNotFoundNotification();
            return default;
        }

        userRepository.Delete(user);

        return default;
    }

    private void AddUserNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.UserNotFound);
    }

    private void AddCanNotProceedNotification()
    {
        notificationManager.Add(NotificationType.BusinessRule, Resources.CannotDeleteUser);
    }
}