using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Categories;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Categories;

public class DeleteCategoryCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryRepository _categoryRepository;

    private readonly DeleteCategoryCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;

    public DeleteCategoryCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _commandHandler = new DeleteCategoryCommandHandler(_notificationManager, _categoryRepository);
    }

    [Fact]
    public async Task Handle_should_delete_a_category_when_it_exists()
    {
        // Arrange
        var command = Fixture.Create<DeleteCategoryCommand>();
        var category = new CategoryBuilder()
            .Build();

        _categoryRepository
            .GetByIdAsync(command.CategoryId)
            .Returns(category);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _categoryRepository
            .Received(1)
            .Delete(category);
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_not_delete_a_category_when_it_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<DeleteCategoryCommand>();

        _categoryRepository
            .GetByIdAsync(command.CategoryId)
            .ReturnsNull();

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _categoryRepository
            .DidNotReceiveWithAnyArgs()
            .Delete(Arg.Any<Category>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.CategoryNotFound);
    }
}