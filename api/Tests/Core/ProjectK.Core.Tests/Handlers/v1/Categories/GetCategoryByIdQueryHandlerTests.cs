using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Handlers.v1.Categories;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Queries.v1.Categories;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Categories;

public class GetCategoryByIdQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryRepository _categoryRepository;

    private readonly GetCategoryByIdQueryHandler _commandHandler;

    private readonly INotificationManager _notificationManager;

    public GetCategoryByIdQueryHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _commandHandler = new GetCategoryByIdQueryHandler(_notificationManager, _categoryRepository);
    }

    [Fact]
    public async Task Handle_should_return_a_category_when_it_exists()
    {
        // Arrange
        var command = Fixture.Create<GetCategoryByIdQuery>();
        var category = new CategoryBuilder()
            .Build();

        _categoryRepository
            .GetByIdAsync(command.CategoryId)
            .Returns(category);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(category);
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_return_null_when_category_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<GetCategoryByIdQuery>();

        _categoryRepository
            .GetByIdAsync(command.CategoryId)
            .ReturnsNull();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.CategoryNotFound);
    }
}