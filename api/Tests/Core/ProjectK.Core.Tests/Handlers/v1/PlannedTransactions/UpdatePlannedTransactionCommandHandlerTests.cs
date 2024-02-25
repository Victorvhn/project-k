using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.PlannedTransactions.Update;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Core.Handlers.v1.PlannedTransactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Core.Services.v1.Interfaces;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.PlannedTransactions;

public class UpdatePlannedTransactionCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryService _categoryService;

    private readonly UpdatePlannedTransactionCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IPlannedTransactionRepository _plannedTransactionRepository;

    public UpdatePlannedTransactionCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();
        _categoryService = Substitute.For<ICategoryService>();

        _commandHandler =
            new UpdatePlannedTransactionCommandHandler(_notificationManager, _plannedTransactionRepository,
                _categoryService);
    }


    [Fact]
    public async Task Handle_UpdatePlannedTransactionCommand_should_not_update_when_planned_transaction_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<UpdatePlannedTransactionCommand>();

        _plannedTransactionRepository
            .GetByIdWithCustomPlannedAsync(command.PlannedTransactionId)
            .ReturnsNull();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }

    [Fact]
    public async Task Handle_UpdatePlannedTransactionCommand_should_not_update_when_category_is_invalid()
    {
        // Arrange
        var plannedTransaction = new PlannedTransactionBuilder()
            .Build();
        var command = Fixture.Build<UpdatePlannedTransactionCommand>()
            .With(w => w.PlannedTransactionId, plannedTransaction.Id)
            .Create();

        _plannedTransactionRepository
            .GetByIdWithCustomPlannedAsync(command.PlannedTransactionId)
            .Returns(plannedTransaction);

        _categoryService
            .ExistsByIdAsync(command.CategoryId.GetValueOrDefault())
            .Returns(false);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.BadRequest, Resources.CategoryNotFound);
    }

    [Fact]
    public async Task
        Handle_UpdatePlannedTransactionCommand_should_not_update_when_provided_startAt_is_being_changed_and_there_is_at_least_one_custom_planned_transaction()
    {
        // Arrange
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithCustomPlannedTransactions(new CustomPlannedTransactionBuilder().Build())
            .WithStartsAt(new DateOnly(2024, 01, 05))
            .Build();
        var command = Fixture.Build<UpdatePlannedTransactionCommand>()
            .With(w => w.PlannedTransactionId, plannedTransaction.Id)
            .With(w => w.StartsAt, new DateOnly(2023, 12, 05))
            .Create();

        _plannedTransactionRepository
            .GetByIdWithCustomPlannedAsync(command.PlannedTransactionId)
            .Returns(plannedTransaction);

        _categoryService
            .ExistsByIdAsync(command.CategoryId.GetValueOrDefault())
            .Returns(true);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.BadRequest, Resources.CanNotUpdateStartsAt);
    }

    [Fact]
    public async Task
        Handle_UpdatePlannedTransactionCommand_should_not_notify_when_there_are_none_custom_planned_transaction()
    {
        // Arrange
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithoutCustomPlannedTransactions()
            .WithStartsAt(new DateOnly(2023, 12, 05))
            .Build();
        var command = Fixture.Build<UpdatePlannedTransactionCommand>()
            .With(w => w.PlannedTransactionId, plannedTransaction.Id)
            .With(w => w.StartsAt, new DateOnly(2023, 12, 05))
            .Create();

        _plannedTransactionRepository
            .GetByIdWithCustomPlannedAsync(command.PlannedTransactionId)
            .Returns(plannedTransaction);

        _categoryService
            .ExistsByIdAsync(command.CategoryId.GetValueOrDefault())
            .Returns(true);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.StartsAt == command.StartsAt));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_UpdatePlannedTransactionCommand_should_update_all()
    {
        // Arrange
        var custom = new CustomPlannedTransactionBuilder()
            .Build();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2024, 01, 13))
            .WithCustomPlannedTransactions(custom)
            .Build();
        var command = Fixture.Build<UpdatePlannedTransactionCommand>()
            .With(w => w.PlannedTransactionId, plannedTransaction.Id)
            .With(w => w.StartsAt, new DateOnly(2024, 01, 13))
            .With(w => w.CategoryId, (Ulid?)null)
            .Create();

        _plannedTransactionRepository
            .GetByIdWithCustomPlannedAsync(command.PlannedTransactionId)
            .Returns(plannedTransaction);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Description == command.Description &&
                a.Amount == command.Amount &&
                a.AmountType == command.AmountType &&
                a.Type == command.Type &&
                a.Recurrence == command.Recurrence &&
                a.StartsAt == plannedTransaction.StartsAt &&
                a.EndsAt == command.EndsAt &&
                a.CategoryId == null &&
                a.CustomPlannedTransactions.Count == 1 &&
                a.CustomPlannedTransactions.ElementAt(0).Description == command.Description &&
                a.CustomPlannedTransactions.ElementAt(0).Amount == command.Amount &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo == custom.RefersTo
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task
        Handle_UpdateMonthlyPlannedTransactionCommand_should_not_update_when_planned_transaction_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<UpdateMonthlyPlannedTransactionCommand>();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .ReturnsNull();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }

    [Fact]
    public async Task Handle_UpdateMonthlyPlannedTransactionCommand_should_update_just_one_when_there_is_none_custom()
    {
        // Arrange
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithoutCustomPlannedTransactions()
            .WithStartsAt(new DateOnly(2020, 12, 01))
            .Build();
        var command = Fixture.Build<UpdateMonthlyPlannedTransactionCommand>()
            .With(w => w.Year, 2024)
            .With(w => w.Month, 01)
            .Create();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Description == plannedTransaction.Description &&
                a.Amount == plannedTransaction.Amount &&
                a.AmountType == plannedTransaction.AmountType &&
                a.Type == plannedTransaction.Type &&
                a.Recurrence == plannedTransaction.Recurrence &&
                a.StartsAt == plannedTransaction.StartsAt &&
                a.EndsAt == plannedTransaction.EndsAt &&
                a.CategoryId == plannedTransaction.CategoryId &&
                a.CustomPlannedTransactions.Count == 1 &&
                a.CustomPlannedTransactions.ElementAt(0).Description == command.Description &&
                a.CustomPlannedTransactions.ElementAt(0).Amount == command.Amount &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Year == command.Year &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Month == command.Month &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Day == command.StartsAt.Day
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_UpdateMonthlyPlannedTransactionCommand_should_update_just_one_when_there_is_a_custom()
    {
        // Arrange
        var custom = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2024, 01, 15))
            .Build();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2022, 10, 15))
            .WithCustomPlannedTransactions(custom)
            .Build();
        var command = Fixture.Build<UpdateMonthlyPlannedTransactionCommand>()
            .With(w => w.Year, 2023)
            .With(w => w.Month, 01)
            .Create();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Description == plannedTransaction.Description &&
                a.Amount == plannedTransaction.Amount &&
                a.AmountType == plannedTransaction.AmountType &&
                a.Type == plannedTransaction.Type &&
                a.Recurrence == plannedTransaction.Recurrence &&
                a.StartsAt == plannedTransaction.StartsAt &&
                a.EndsAt == plannedTransaction.EndsAt &&
                a.CategoryId == plannedTransaction.CategoryId &&
                a.CustomPlannedTransactions.Count == 1 &&
                a.CustomPlannedTransactions.ElementAt(0).Description == command.Description &&
                a.CustomPlannedTransactions.ElementAt(0).Amount == command.Amount &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Year == custom.RefersTo.Year &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Month == custom.RefersTo.Month &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Day == custom.RefersTo.Day
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task
        Handle_UpdateFromNowOnPlannedTransaction_should_not_update_when_planned_transaction_does_not_exist()
    {
        // Arrange
        var command = Fixture.Build<UpdateFromNowOnPlannedTransactionCommand>()
            .With(w => w.Year, 2023)
            .With(w => w.Month, 12)
            .Create();

        _plannedTransactionRepository
            .GetByIdWithAllCustomPlannedBeforeDateAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .ReturnsNull();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }

    [Fact]
    public async Task Handle_UpdateFromNowOnPlannedTransaction_should_update_from_now_on_for_monthly_transaction()
    {
        // Arrange
        var custom1 = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2023, 12, 01))
            .Build();
        var custom2 = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2023, 11, 01))
            .Build();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 06, 01))
            .WithCustomPlannedTransactions(custom1)
            .WithCustomPlannedTransactions(custom2)
            .WithRecurrence(Recurrence.Monthly)
            .Build();
        var command = Fixture.Build<UpdateFromNowOnPlannedTransactionCommand>()
            .With(w => w.Year, 2023)
            .With(w => w.Month, 12)
            .With(w => w.StartsAt, new DateOnly(2023, 06, 05))
            .With(w => w.CategoryId, (Ulid?)null)
            .Create();

        _plannedTransactionRepository
            .GetByIdWithAllCustomPlannedBeforeDateAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        result
            .Should()
            .BeEquivalentTo(command, opt => opt.ExcludingMissingMembers().Excluding(e => e.StartsAt));
        result!
            .StartsAt
            .Should()
            .Be(new DateOnly(plannedTransaction.StartsAt.Year, plannedTransaction.StartsAt.Month, command.StartsAt.Day));
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Description == command.Description &&
                a.Amount == command.Amount &&
                a.AmountType == command.AmountType &&
                a.Type == command.Type &&
                a.Recurrence == command.Recurrence &&
                a.StartsAt.Year == plannedTransaction.StartsAt.Year &&
                a.StartsAt.Month == plannedTransaction.StartsAt.Month &&
                a.StartsAt.Day == command.StartsAt.Day &&
                a.EndsAt == command.EndsAt &&
                a.CategoryId == command.CategoryId &&
                a.CustomPlannedTransactions.Count == 7
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_update_from_now_on_for_annual_transaction()
    {
        // Arrange
        var custom = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2023, 12, 01))
            .Build();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2020, 12, 01))
            .WithCustomPlannedTransactions(custom)
            .WithRecurrence(Recurrence.Annual)
            .Build();
        var command = Fixture.Build<UpdateFromNowOnPlannedTransactionCommand>()
            .With(w => w.Year, 2023)
            .With(w => w.Month, 12)
            .With(w => w.StartsAt, new DateOnly(2023, 12, 05))
            .With(w => w.CategoryId, (Ulid?)null)
            .Create();

        _plannedTransactionRepository
            .GetByIdWithAllCustomPlannedBeforeDateAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Description == command.Description &&
                a.Amount == command.Amount &&
                a.AmountType == command.AmountType &&
                a.Type == command.Type &&
                a.Recurrence == command.Recurrence &&
                a.StartsAt.Year == plannedTransaction.StartsAt.Year &&
                a.StartsAt.Month == plannedTransaction.StartsAt.Month &&
                a.StartsAt.Day == command.StartsAt.Day &&
                a.EndsAt == command.EndsAt &&
                a.CategoryId == command.CategoryId &&
                a.CustomPlannedTransactions.Count == 4
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }
}