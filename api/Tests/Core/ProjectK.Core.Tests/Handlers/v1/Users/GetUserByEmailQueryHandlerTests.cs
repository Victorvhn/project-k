using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Handlers.v1.Users;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Queries.v1.Users;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Users;

public class GetUserByEmailQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly GetUserByEmailQueryHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IUserRepository _userRepository;

    public GetUserByEmailQueryHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _userRepository = Substitute.For<IUserRepository>();

        _commandHandler = new GetUserByEmailQueryHandler(_notificationManager, _userRepository);
    }

    [Fact]
    public async Task Handle_should_return_the_user_when_it_exists()
    {
        // Arrange
        var query = Fixture.Create<GetUserByEmailQuery>();
        var user = new UserBuilder()
            .Build();

        _userRepository
            .GetByEmailAsync(query.Email)
            .Returns(user);

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(user, opt => opt.ExcludingMissingMembers());
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_return_null_when_user_does_not_exist()
    {
        // Arrange
        var query = Fixture.Create<GetUserByEmailQuery>();

        _userRepository
            .GetByEmailAsync(query.Email)
            .ReturnsNull();

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, string.Format(Resources.UserWithEmailNotFound, query.Email));
    }
}