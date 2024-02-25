using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.PlannedTransactions.Delete;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.PlannedTransactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.PlannedTransactions;

public class DeletePlannedTransactionCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly DeletePlannedTransactionCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IPlannedTransactionRepository _plannedTransactionRepository;

    public DeletePlannedTransactionCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();

        _commandHandler =
            new DeletePlannedTransactionCommandHandler(_notificationManager,
                _plannedTransactionRepository);
    }

    [Fact]
    public async Task
        Handle_DeletePlannedTransactionCommand_should_return_null_when_planned_transaction_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<DeletePlannedTransactionCommand>();

        _plannedTransactionRepository
            .GetByIdAsync(command.PlannedTransactionId)
            .ReturnsNull();

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }

    [Fact]
    public async Task Handle_DeletePlannedTransactionCommand_should_delete_all_planned_transaction()
    {
        // Arrange
        var plannedTransaction = new PlannedTransactionBuilder()
            .Build();
        var command = Fixture.Build<DeletePlannedTransactionCommand>()
            .With(w => w.PlannedTransactionId, plannedTransaction.Id)
            .Create();

        _plannedTransactionRepository
            .GetByIdAsync(command.PlannedTransactionId)
            .Returns(plannedTransaction);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _plannedTransactionRepository
            .Received(1)
            .Delete(plannedTransaction);
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task
        Handle_DeleteMonthlyPlannedTransactionCommand_should_notify_when_planned_transaction_does_not_exist()
    {
        // Arrange
        var command = Fixture.Build<DeleteMonthlyPlannedTransactionCommand>()
            .With(w => w.Year, 2024)
            .With(w => w.Month, 01)
            .Create();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .ReturnsNull();

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }

    [Fact]
    public async Task
        Handle_DeleteFromNowOnPlannedTransaction_should_notify_when_planned_transaction_does_not_exist()
    {
        // Arrange
        var command = Fixture.Build<DeleteFromNowOnPlannedTransaction>()
            .With(w => w.Year, 2024)
            .With(w => w.Month, 01)
            .Create();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .ReturnsNull();

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }

    [Fact]
    public async Task
        Handle_DeleteMonthlyPlannedTransactionCommand_should_delete_just_one_planned_transaction_when_there_are_no_custom_transactions()
    {
        // Arrange
        var command = Fixture.Build<DeleteMonthlyPlannedTransactionCommand>()
            .With(w => w.Year, 2024)
            .With(w => w.Month, 01)
            .Create();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2020, 12, 05))
            .WithEndsAt(new DateOnly(2030, 12, 05))
            .Build();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        var expectedEndDate = new DateOnly(command.Year, command.Month, plannedTransaction.StartsAt.Day);
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Id == plannedTransaction.Id &&
                a.CustomPlannedTransactions.Count == 1 &&
                a.CustomPlannedTransactions.ElementAt(0).Description == plannedTransaction.Description &&
                a.CustomPlannedTransactions.ElementAt(0).Amount == 0 &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Year == expectedEndDate.Year &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Month == expectedEndDate.Month &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Day == expectedEndDate.Day &&
                a.CustomPlannedTransactions.ElementAt(0).Active == false));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task
        Handle_DeleteMonthlyPlannedTransactionCommand_should_delete_just_one_planned_transaction_when_there_are_custom_transactions()
    {
        // Arrange
        var command = Fixture.Build<DeleteMonthlyPlannedTransactionCommand>()
            .With(w => w.Year, 2024)
            .With(w => w.Month, 01)
            .Create();
        var customPlannedTransaction = new CustomPlannedTransactionBuilder()
            .Active()
            .WithRefersTo(new DateOnly(2023, 12, 05))
            .Build();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2020, 07, 05))
            .WithCustomPlannedTransactions(customPlannedTransaction)
            .Build();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Id == plannedTransaction.Id &&
                a.CustomPlannedTransactions.Count == 1 &&
                a.CustomPlannedTransactions.ElementAt(0).Description == customPlannedTransaction.Description &&
                a.CustomPlannedTransactions.ElementAt(0).Amount == customPlannedTransaction.Amount &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Year == customPlannedTransaction.RefersTo.Year &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Month == customPlannedTransaction.RefersTo.Month &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Day == customPlannedTransaction.RefersTo.Day &&
                a.CustomPlannedTransactions.ElementAt(0).Active == false));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task
        Handle_DeleteFromNowOnPlannedTransaction_should_delete_from_now_on_planned_transaction_when_there_are_no_custom_transactions()
    {
        // Arrange
        var command = Fixture.Build<DeleteFromNowOnPlannedTransaction>()
            .With(w => w.Year, 2024)
            .With(w => w.Month, 01)
            .Create();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2020, 12, 05))
            .Build();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        var expectedEndDate = new DateOnly(command.Year, command.Month, plannedTransaction.StartsAt.Day)
            .AddMonths(-1);
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Id == plannedTransaction.Id &&
                a.EndsAt != null &&
                a.EndsAt.Value.Year == expectedEndDate.Year &&
                a.EndsAt.Value.Month == expectedEndDate.Month &&
                a.EndsAt.Value.Day == expectedEndDate.Day &&
                a.CustomPlannedTransactions.Count == 0
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(default, Arg.Any<string>());
    }

    [Fact]
    public async Task
        Handle_DeleteFromNowOnPlannedTransaction_should_delete_from_now_on_planned_transaction_when_there_are_custom_transactions()
    {
        // Arrange
        var customPlannedTransaction = new CustomPlannedTransactionBuilder()
            .Active()
            .WithRefersTo(new DateOnly(2023, 12, 05))
            .Build();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2020, 07, 05))
            .WithCustomPlannedTransactions(customPlannedTransaction)
            .Build();
        var command = Fixture.Build<DeleteFromNowOnPlannedTransaction>()
            .With(w => w.PlannedTransactionId, plannedTransaction.Id)
            .With(w => w.Year, 2023)
            .With(w => w.Month, 12)
            .Create();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        var expectedEndDate = new DateOnly(command.Year, command.Month, plannedTransaction.StartsAt.Day)
            .AddMonths(-1);
        _plannedTransactionRepository
            .Received(1)
            .Update(Arg.Is<PlannedTransaction>(a =>
                a.Id == plannedTransaction.Id &&
                a.EndsAt != null &&
                a.EndsAt.Value.Year == expectedEndDate.Year &&
                a.EndsAt.Value.Month == expectedEndDate.Month &&
                a.EndsAt.Value.Day == expectedEndDate.Day &&
                a.CustomPlannedTransactions.Count == 1 &&
                a.CustomPlannedTransactions.ElementAt(0).Description == customPlannedTransaction.Description &&
                a.CustomPlannedTransactions.ElementAt(0).Amount == customPlannedTransaction.Amount &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Year == customPlannedTransaction.RefersTo.Year &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Month == customPlannedTransaction.RefersTo.Month &&
                a.CustomPlannedTransactions.ElementAt(0).RefersTo.Day == customPlannedTransaction.RefersTo.Day &&
                a.CustomPlannedTransactions.ElementAt(0).Active == false
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(default, Arg.Any<string>());
    }
}