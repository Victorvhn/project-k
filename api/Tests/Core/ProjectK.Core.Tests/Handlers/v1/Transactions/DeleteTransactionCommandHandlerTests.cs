using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Transactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Transactions;

public class DeleteTransactionCommandHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly DeleteTransactionCommandHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly ITransactionRepository _transactionRepository;

    public DeleteTransactionCommandHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _transactionRepository = Substitute.For<ITransactionRepository>();

        _commandHandler =
            new DeleteTransactionCommandHandler(_notificationManager, _transactionRepository);
    }


    [Fact]
    public async Task Handle_should_delete_a_transaction()
    {
        // Arrange
        var transaction = new TransactionBuilder()
            .Build();
        var command = Fixture.Build<DeleteTransactionCommand>()
            .With(w => w.TransactionId, transaction.Id)
            .Create();

        _transactionRepository
            .GetByIdAsync(command.TransactionId)
            .Returns(transaction);

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _transactionRepository
            .Received(1)
            .Delete(transaction);
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_not_delete_a_transaction_when_it_does_not_exist()
    {
        // Arrange
        var command = Fixture.Create<DeleteTransactionCommand>();

        _transactionRepository
            .GetByIdAsync(command.TransactionId)
            .ReturnsNull();

        // Act
        await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.TransactionNotFound);
        _transactionRepository
            .DidNotReceiveWithAnyArgs()
            .Delete(Arg.Any<Transaction>());
    }
}