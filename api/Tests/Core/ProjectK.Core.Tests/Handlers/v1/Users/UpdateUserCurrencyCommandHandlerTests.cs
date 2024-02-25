using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Core.Handlers.v1.Users;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Users;

public class UpdateUserCurrencyCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly UpdateUserCurrencyCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;

    public UpdateUserCurrencyCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _userRepository = Substitute.For<IUserRepository>();
        _userContext = Substitute.For<IUserContext>();

        _commandHandler =
            new UpdateUserCurrencyCommandHandler(_userContext, _notificationManager, _userRepository);
    }


    [Fact]
    public async Task Handle_should_update_the_users_currency_when_it_exists()
    {
        // Arrange
        var user = UserBuilder
            .Create();
        var command = Fixture.Build<UpdateUserCurrencyCommand>()
            .With(w => w.UserId, user.Id)
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
            .Update(Arg.Is<User>(a =>
                a.Id == command.UserId &&
                a.Currency == command.Currency));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_not_delete_a_user_when_it_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<UpdateUserCurrencyCommand>();

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
            .Update(Arg.Any<User>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.UserNotFound);
    }

    [Fact]
    public async Task Handle_should_not_delete_a_user_when_user_id_does_not_match_with_user_context_id()
    {
        // Arrange
        var command = Fixture.Create<UpdateUserCurrencyCommand>();
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
            .Update(Arg.Any<User>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.BusinessRule, Resources.CannotDeleteUser);
    }
}