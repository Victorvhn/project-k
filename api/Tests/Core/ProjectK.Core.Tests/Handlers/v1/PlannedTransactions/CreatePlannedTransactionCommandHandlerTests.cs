using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.PlannedTransactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.PlannedTransactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Core.Services.v1.Interfaces;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Handlers.v1.PlannedTransactions;

public class CreatePlannedTransactionCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryService _categoryService;

    private readonly CreatePlannedTransactionCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IPlannedTransactionRepository _plannedTransactionRepository;

    public CreatePlannedTransactionCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();
        _categoryService = Substitute.For<ICategoryService>();

        _commandHandler =
            new CreatePlannedTransactionCommandHandler(_notificationManager, _plannedTransactionRepository,
                _categoryService);
    }

    [Fact]
    public async Task CreateAsync_should_create_a_planned_transaction()
    {
        // Arrange
        var command = Fixture.Create<CreatePlannedTransactionCommand>();

        _categoryService
            .ExistsByIdAsync(command.CategoryId.GetValueOrDefault())
            .Returns(true);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(command, opt => opt.ExcludingMissingMembers());
        await _plannedTransactionRepository
            .Received(1)
            .AddAsync(Arg.Is<PlannedTransaction>(a =>
                a.Description == command.Description &&
                a.Amount == command.Amount &&
                a.AmountType == command.AmountType &&
                a.Type == command.Type &&
                a.Recurrence == command.Recurrence &&
                a.StartsAt == command.StartsAt &&
                a.EndsAt == command.EndsAt &&
                a.CategoryId == command.CategoryId
            ));
    }

    [Fact]
    public async Task
        CreateAsync_should_not_create_a_planned_transaction_when_category_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<CreatePlannedTransactionCommand>();

        _categoryService
            .ExistsByIdAsync(command.CategoryId.GetValueOrDefault())
            .Returns(false);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        await _plannedTransactionRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<PlannedTransaction>());
        _notificationManager
            .Received(1)
            .Add(NotificationType.BadRequest, Resources.CategoryNotFound);
    }

    [Fact]
    public async Task CreateAsync_should_create_when_category_is_null()
    {
        // Arrange
        var command = Fixture.Build<CreatePlannedTransactionCommand>()
            .With(w => w.CategoryId, (Ulid?)null)
            .Create();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(command, opt => opt.ExcludingMissingMembers());
        await _categoryService
            .DidNotReceiveWithAnyArgs()
            .ExistsByIdAsync(Arg.Any<Ulid>(), Arg.Any<CancellationToken>());
        await _plannedTransactionRepository
            .Received(1)
            .AddAsync(Arg.Is<PlannedTransaction>(a =>
                a.Description == command.Description &&
                a.Amount == command.Amount &&
                a.AmountType == command.AmountType &&
                a.Type == command.Type &&
                a.Recurrence == command.Recurrence &&
                a.StartsAt == command.StartsAt &&
                a.EndsAt == command.EndsAt &&
                a.CategoryId == null
            ));
    }
}