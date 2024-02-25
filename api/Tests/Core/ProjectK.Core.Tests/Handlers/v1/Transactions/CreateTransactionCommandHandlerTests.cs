using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Transactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Core.Services.v1.Interfaces;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Handlers.v1.Transactions;

public class CreateTransactionCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryService _categoryService;

    private readonly CreateTransactionCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IPlannedTransactionService _plannedTransactionService;
    private readonly ITransactionRepository _transactionRepository;

    public CreateTransactionCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _categoryService = Substitute.For<ICategoryService>();
        _plannedTransactionService = Substitute.For<IPlannedTransactionService>();

        _commandHandler = new CreateTransactionCommandHandler(_notificationManager, _transactionRepository,
            _categoryService, _plannedTransactionService);
    }

    [Fact]
    public async Task Handle_should_create_a_transaction()
    {
        // Arrange
        var command = Fixture.Build<CreateTransactionCommand>()
            .With(w => w.CategoryId, (Ulid?)null)
            .With(w => w.PlannedTransactionId, (Ulid?)null)
            .Create();

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        await _transactionRepository
            .Received(1)
            .AddAsync(Arg.Is<Transaction>(a =>
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
    public async Task Handle_should_not_create_a_transaction_when_category_is_invalid()
    {
        // Arrange
        var command = Fixture.Create<CreateTransactionCommand>();

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
        await _transactionRepository
            .DidNotReceiveWithAnyArgs()
            .AddAsync(Arg.Any<Transaction>());
    }

    [Fact]
    public async Task Handle_should_not_create_a_transaction_when_planned_transaction_is_invalid()
    {
        // Arrange
        var command = Fixture.Create<CreateTransactionCommand>();

        _categoryService
            .ExistsByIdAsync(command.CategoryId!.Value)
            .Returns(true);

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
        await _transactionRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<Transaction>());
    }
}