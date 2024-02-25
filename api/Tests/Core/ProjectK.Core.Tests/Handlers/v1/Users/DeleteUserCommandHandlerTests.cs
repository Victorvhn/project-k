using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Users;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Users;

public class DeleteUserCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly DeleteUserCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _userRepository = Substitute.For<IUserRepository>();
        _userContext = Substitute.For<IUserContext>();

        _commandHandler =
            new DeleteUserCommandHandler(_notificationManager, _userRepository, _userContext);
    }


    [Fact]
    public async Task Handle_should_delete_a_user_when_it_exists()
    {
        // Arrange
        var command = Fixture.Create<DeleteUserCommand>();
        var user = UserBuilder
            .Create();

        _userContext
            .UserId
            .Returns(command.UserId);
        _userRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _userRepository
            .Received(1)
            .Delete(user);
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_not_delete_a_user_when_it_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<DeleteUserCommand>();

        _userContext
            .UserId
            .Returns(command.UserId);
        _userRepository
            .GetByIdAsync(command.UserId)
            .ReturnsNull();

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _userRepository
            .DidNotReceiveWithAnyArgs()
            .Delete(Arg.Any<User>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.UserNotFound);
    }

    [Fact]
    public async Task Handle_should_not_delete_a_user_when_user_id_does_not_match_with_user_context_id()
    {
        // Arrange
        var command = Fixture.Create<DeleteUserCommand>();
        var user = UserBuilder
            .Create();

        _userContext
            .UserId
            .Returns(Ulid.NewUlid());
        _userRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _userRepository
            .DidNotReceiveWithAnyArgs()
            .Delete(Arg.Any<User>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.BusinessRule, Resources.CannotDeleteUser);
    }
}