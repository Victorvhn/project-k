using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Services.v1;
using ProjectK.Core.Services.v1.Interfaces;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Services.v1;

public class CategoryServiceTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryRepository _categoryRepository;

    private readonly ICategoryService _categoryService;

    public CategoryServiceTests()
    {
        var notificationManager = Substitute.For<INotificationManager>();
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _categoryService = new CategoryService(notificationManager, _categoryRepository);
    }

    [Fact]
    public async Task ExistsByIdAsync_should_return_true_when_category_exists()
    {
        // Arrange
        var id = Fixture.Create<Ulid>();
        var cancellationToken = new CancellationToken();

        _categoryRepository
            .ExistsByIdAsync(id, cancellationToken)
            .Returns(true);

        // Act
        var result = await _categoryService.ExistsByIdAsync(id, cancellationToken);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsByIdAsync_should_return_false_when_category_does_not_exist()
    {
        // Arrange
        var id = Fixture.Create<Ulid>();
        var cancellationToken = new CancellationToken();

        _categoryRepository
            .ExistsByIdAsync(id, cancellationToken)
            .Returns(false);

        // Act
        var result = await _categoryService.ExistsByIdAsync(id, cancellationToken);

        // Assert
        result
            .Should()
            .BeFalse();
    }
}