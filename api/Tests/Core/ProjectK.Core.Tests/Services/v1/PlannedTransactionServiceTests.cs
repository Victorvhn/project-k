using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Services.v1;
using ProjectK.Core.Services.v1.Interfaces;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Services.v1;

public class PlannedTransactionServiceTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly IPlannedTransactionRepository _plannedTransactionRepository;

    private readonly IPlannedTransactionService _plannedTransactionService;

    public PlannedTransactionServiceTests()
    {
        var notificationManager = Substitute.For<INotificationManager>();
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();

        _plannedTransactionService =
            new PlannedTransactionService(notificationManager, _plannedTransactionRepository);
    }

    [Fact]
    public async Task ExistsByIdAsync_should_return_false_when_planned_transaction_does_not_exist()
    {
        // Arrange
        var id = Fixture.Create<Ulid>();

        _plannedTransactionRepository
            .ExistsByIdAsync(id)
            .Returns(false);

        // Act
        var result = await _plannedTransactionService.ExistsByIdAsync(id);

        // Assert
        result
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task ExistsByIdAsync_should_return_true_when_planned_transaction_exists()
    {
        // Arrange
        var id = Fixture.Create<Ulid>();

        _plannedTransactionRepository
            .ExistsByIdAsync(id)
            .Returns(true);

        // Act
        var result = await _plannedTransactionService.ExistsByIdAsync(id);

        // Assert
        result
            .Should()
            .BeTrue();
    }
}