using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Core.Handlers.v1.Users;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Handlers.v1.Users;

public class CreateUserCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly CreateUserCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _userRepository = Substitute.For<IUserRepository>();

        _commandHandler = new CreateUserCommandHandler(_notificationManager, _userRepository);
    }

    [Fact]
    public async Task Handle_should_create_a_user_when_email_is_not_in_use()
    {
        // Arrange
        var data = Fixture.Create<CreateUserCommand>();

        _userRepository
            .ExistsByEmailAsync(data.Email)
            .Returns(false);

        // Act
        var result = await _commandHandler.Handle(data, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(data, opt => opt.ExcludingMissingMembers());

        await _userRepository
            .Received(1)
            .AddAsync(Arg.Is<User>(a =>
                a.Name == data.Name &&
                a.Email == data.Email &&
                a.Currency == Currency.Brl
            ));
    }

    [Fact]
    public async Task CreateUserAsync_should_not_create_a_user_when_email_is_in_use()
    {
        // Arrange
        var data = Fixture.Create<CreateUserCommand>();

        _userRepository
            .ExistsByEmailAsync(data.Email)
            .Returns(true);

        // Act
        var result = await _commandHandler.Handle(data, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        await _userRepository
            .DidNotReceiveWithAnyArgs()
            .AddAsync(Arg.Any<User>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.BusinessRule, string.Format(Resources.EmailAlreadyInUse, data.Email));
    }
}