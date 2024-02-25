using AutoFixture;
using FluentAssertions;
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

namespace ProjectK.Core.Tests.Handlers.v1.Categories;

public class UpdateCategoryCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryRepository _categoryRepository;

    private readonly UpdateCategoryCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;

    public UpdateCategoryCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _commandHandler = new UpdateCategoryCommandHandler(_notificationManager, _categoryRepository);
    }

    [Fact]
    public async Task Handle_should_update_a_category_when_it_exists()
    {
        // Arrange
        var category = Fixture.Create<Category>();
        var command = Fixture.Build<UpdateCategoryCommand>()
            .With(w => w.CategoryId, category.Id)
            .Create();

        _categoryRepository
            .GetByIdAsync(command.CategoryId)
            .Returns(category);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(category, opt => opt.ExcludingMissingMembers());
        _categoryRepository
            .Received(1)
            .Update(Arg.Is<Category>(a =>
                a.Id == category.Id &&
                a.Name == command.Name &&
                a.HexColor == command.HexColor
            ));
    }

    [Fact]
    public async Task Handle_should_not_update_a_category_when_it_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<UpdateCategoryCommand>();

        _categoryRepository
            .GetByIdAsync(command.CategoryId)
            .ReturnsNull();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _categoryRepository
            .DidNotReceiveWithAnyArgs()
            .Update(Arg.Any<Category>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, string.Format(Resources.CategoryNameNotFound, command.Name));
    }
}