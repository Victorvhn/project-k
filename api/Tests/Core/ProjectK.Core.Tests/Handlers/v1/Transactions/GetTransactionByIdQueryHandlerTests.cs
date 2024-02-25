using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Handlers.v1.Transactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Queries.v1.Transactions;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Transactions;

public class GetTransactionByIdQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly GetTransactionByIdQueryHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionByIdQueryHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _transactionRepository = Substitute.For<ITransactionRepository>();

        _commandHandler = new GetTransactionByIdQueryHandler(_notificationManager, _transactionRepository);
    }

    [Fact]
    public async Task Handle_should_return_a_transaction()
    {
        // Arrange
        var query = Fixture.Create<GetTransactionByIdQuery>();
        var transaction = new TransactionBuilder()
            .Build();

        _transactionRepository
            .GetByIdAsync(query.TransactionId)
            .Returns(transaction);

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        result!
            .Should()
            .BeEquivalentTo(transaction);
    }

    [Fact]
    public async Task Handle_should_return_null_when_transaction_does_not_exist()
    {
        // Arrange
        var query = Fixture.Create<GetTransactionByIdQuery>();

        _transactionRepository
            .GetByIdAsync(query.TransactionId)
            .ReturnsNull();

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.TransactionNotFound);
    }
}