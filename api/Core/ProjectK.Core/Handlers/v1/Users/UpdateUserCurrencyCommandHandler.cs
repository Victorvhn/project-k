using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Users;

internal sealed class UpdateUserCurrencyCommandHandler(
    IUserContext userContext,
    INotificationManager notificationManager,
    IUserRepository userRepository) : ICommandHandler<UpdateUserCurrencyCommand, User?>
{
    public async ValueTask<User?> Handle(UpdateUserCurrencyCommand command, CancellationToken cancellationToken)
    {
        if (userContext.UserId != command.UserId)
        {
            AddCanNotProceedNotification();
            return default;
        }
        
        var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
        {
            AddUserNotFoundNotification();
            return default;
        }
        
        user.UpdateCurrency(command.Currency);

        userRepository.Update(user);
        
        return user;
    }
    
    private void AddCanNotProceedNotification()
    {
        notificationManager.Add(NotificationType.BusinessRule, Resources.CannotDeleteUser);
    }
    
    private void AddUserNotFoundNotification()
    {
        notificationManager.Add(NotificationType.NotFound, Resources.UserNotFound);
    }
}