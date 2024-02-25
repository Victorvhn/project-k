using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Transactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Core.Services.v1.Interfaces;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Transactions;

public class UpdateTransactionCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryService _categoryService;

    private readonly UpdateTransactionCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IPlannedTransactionService _plannedTransactionService;
    private readonly ITransactionRepository _transactionRepository;

    public UpdateTransactionCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _categoryService = Substitute.For<ICategoryService>();
        _plannedTransactionService = Substitute.For<IPlannedTransactionService>();

        _commandHandler = new UpdateTransactionCommandHandler(_notificationManager, _transactionRepository,
            _categoryService, _plannedTransactionService);
    }

    [Fact]
    public async Task Handle_should_update_a_transaction()
    {
        // Arrange
        var transaction = new TransactionBuilder()
            .Build();
        var command = Fixture.Build<UpdateTransactionCommand>()
            .With(w => w.TransactionId, transaction.Id)
            .Create();

        _transactionRepository
            .GetByIdAsync(command.TransactionId)
            .Returns(transaction);

        _categoryService
            .ExistsByIdAsync(command.CategoryId!.Value)
            .Returns(true);

        _plannedTransactionService
            .ExistsByIdAsync(command.PlannedTransactionId!.Value)
            .Returns(true);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        _transactionRepository
            .Received(1)
            .Update(Arg.Is<Transaction>(a =>
                a.Id == transaction.Id &&
                a.Description == command.Description &&
                a.Amount == command.Amount &&
                a.Type == command.Type &&
                a.PaidAt == command.PaidAt &&
                a.CategoryId == command.CategoryId &&
                a.PlannedTransactionId == command.PlannedTransactionId
            ));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_not_update_a_transaction_when_it_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<UpdateTransactionCommand>();

        _transactionRepository
            .GetByIdAsync(command.TransactionId)
            .ReturnsNull();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.TransactionNotFound);
        _transactionRepository
            .DidNotReceiveWithAnyArgs()
            .Update(Arg.Any<Transaction>());
    }

    [Fact]
    public async Task Handle_should_not_update_a_transaction_when_category_is_invalid()
    {
        // Arrange
        var transaction = new TransactionBuilder()
            .Build();
        var command = Fixture.Build<UpdateTransactionCommand>()
            .With(w => w.TransactionId, transaction.Id)
            .Create();

        _transactionRepository
            .GetByIdAsync(command.TransactionId)
            .Returns(transaction);

        _categoryService
            .ExistsByIdAsync(command.CategoryId!.Value)
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
        _transactionRepository
            .DidNotReceiveWithAnyArgs()
            .Update(Arg.Any<Transaction>());
    }

    [Fact]
    public async Task Handle_should_not_update_a_transaction_when_planned_transaction_is_invalid()
    {
        // Arrange
        var transaction = new TransactionBuilder()
            .Build();
        var command = Fixture.Build<UpdateTransactionCommand>()
            .With(w => w.TransactionId, transaction.Id)
            .With(w => w.CategoryId, (Ulid?)null)
            .Create();

        _transactionRepository
            .GetByIdAsync(command.TransactionId)
            .Returns(transaction);

        _plannedTransactionService
            .ExistsByIdAsync(command.PlannedTransactionId!.Value)
            .Returns(false);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.BadRequest, Resources.PlannedTransactionNotFound);
        _transactionRepository
            .DidNotReceiveWithAnyArgs()
            .Update(Arg.Any<Transaction>());
        await _categoryService
            .DidNotReceiveWithAnyArgs()
            .ExistsByIdAsync(Arg.Any<Ulid>());
    }
}