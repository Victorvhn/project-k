using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Categories;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Handlers.v1.Categories;

public class CreateCategoryCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryRepository _categoryRepository;

    private readonly CreateCategoryCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;

    public CreateCategoryCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _commandHandler = new CreateCategoryCommandHandler(_notificationManager, _categoryRepository);
    }

    [Fact]
    public async Task Handle_should_create_a_category_when_name_is_not_in_use()
    {
        // Arrange
        var command = Fixture.Create<CreateCategoryCommand>();

        _categoryRepository
            .ExistsByNameAsync(command.Name)
            .Returns(false);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(command, opt => opt.ExcludingMissingMembers());
        await _categoryRepository
            .Received(1)
            .AddAsync(Arg.Is<Category>(a =>
                a.Name == command.Name &&
                a.HexColor == command.HexColor
            ));
    }

    [Fact]
    public async Task Handle_should_not_create_a_category_when_name_is_in_use()
    {
        // Arrange
        var command = Fixture.Create<CreateCategoryCommand>();

        _categoryRepository
            .ExistsByNameAsync(command.Name)
            .Returns(true);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        await _categoryRepository
            .DidNotReceiveWithAnyArgs()
            .AddAsync(Arg.Any<Category>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.Conflict, string.Format(Resources.CategoryAlreadyExists, command.Name));
    }
}