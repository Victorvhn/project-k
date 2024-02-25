using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.PlannedTransactions;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.PlannedTransactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.PlannedTransactions;

public class PayPlannedTransactionCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly PayPlannedTransactionCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IPlannedTransactionRepository _plannedTransactionRepository;
    private readonly ITransactionRepository _transactionRepository;

    public PayPlannedTransactionCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();

        _commandHandler = new PayPlannedTransactionCommandHandler(_notificationManager,
            _transactionRepository,
            _plannedTransactionRepository);
    }

    [Fact]
    public async Task Handle_should_create_a_transaction()
    {
        // Arrange
        var command = Fixture.Build<PayPlannedTransactionCommand>()
            .With(w => w.Year, 2024)
            .With(w => w.Month, 1)
            .Create();
        var plannedTransaction = new PlannedTransactionBuilder()
            .Build();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(command.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == command.Year && a.Month == command.Month))
            .Returns(plannedTransaction);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        await _transactionRepository
            .Received(1)
            .AddAsync(Arg.Is<Transaction>(a =>
                a.Description == plannedTransaction.Description &&
                a.Amount == command.Amount &&
                a.Type == plannedTransaction.Type &&
                a.PaidAt.Month == command.Month &&
                a.PaidAt.Year == command.Year &&
                a.CategoryId == plannedTransaction.CategoryId &&
                a.PlannedTransactionId == plannedTransaction.Id
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_not_create_a_transaction_when_planned_transaction_is_invalid()
    {
        // Arrange
        var command = Fixture.Build<PayPlannedTransactionCommand>()
            .With(w => w.Year, 2024)
            .With(w => w.Month, 1)
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
            .Add(NotificationType.BusinessRule, Resources.UnableToCreateTransaction);
        await _transactionRepository
            .DidNotReceiveWithAnyArgs()
            .AddAsync(Arg.Any<Transaction>());
    }
}