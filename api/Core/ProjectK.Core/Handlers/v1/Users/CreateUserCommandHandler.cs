using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Users;

internal sealed class CreateUserCommandHandler(
    INotificationManager notificationManager,
    IUserRepository userRepository) : ICommandHandler<CreateUserCommand, User?>
{
    public async ValueTask<User?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsByEmailAsync(request.Email, cancellationToken);

        if (userExists)
        {
            AddEmailAlreadyInUseNotification(request.Email);
            return default;
        }

        var user = User.CreateInstance(request);

        await userRepository.AddAsync(user, cancellationToken);

        return user;
    }

    private void AddEmailAlreadyInUseNotification(string email)
    {
        notificationManager.Add(NotificationType.BusinessRule,
            string.Format(Resources.EmailAlreadyInUse, email));
    }
}