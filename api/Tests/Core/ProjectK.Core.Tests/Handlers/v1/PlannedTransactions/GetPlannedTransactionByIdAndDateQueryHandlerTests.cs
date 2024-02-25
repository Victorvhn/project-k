using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Handlers.v1.PlannedTransactions;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Queries.v1.PlannedTransactions;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.PlannedTransactions;

public class GetPlannedTransactionByIdAndDateQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly GetPlannedTransactionByIdAndDateQueryHandler _commandHandler;

    private readonly INotificationManager _notificationManager;
    private readonly IPlannedTransactionRepository _plannedTransactionRepository;
    private readonly ICustomPlannedTransactionRepository _customPlannedTransactionRepository;

    public GetPlannedTransactionByIdAndDateQueryHandlerTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();
        _customPlannedTransactionRepository = Substitute.For<ICustomPlannedTransactionRepository>();
        
        _commandHandler = new GetPlannedTransactionByIdAndDateQueryHandler(_notificationManager,
            _plannedTransactionRepository, _customPlannedTransactionRepository);
    }

    [Fact]
    public async Task Handle_should_return_null_when_planned_transaction_does_not_exist()
    {
        // Arrange
        var query = Fixture.Create<GetPlannedTransactionByIdAndDateQuery>();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(query.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month))
            .ReturnsNull();
        _customPlannedTransactionRepository
            .AnyByPlannedTransactionIdAsync(query.PlannedTransactionId)
            .Returns(false);
        
        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeNull();
        _notificationManager
            .Received(1)
            .Add(NotificationType.NotFound, Resources.PlannedTransactionNotFound);
    }

    [Fact]
    public async Task Handle_should_return_the_custom_planned_transaction()
    {
        // Arrange
        var custom = new CustomPlannedTransactionBuilder()
            .WithoutBasePlannedTransaction()
            .Build();
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithCustomPlannedTransactions(custom)
            .Build();
        var query = Fixture.Build<GetPlannedTransactionByIdAndDateQuery>()
            .With(w => w.PlannedTransactionId, plannedTransaction.Id)
            .Create();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(query.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month))
            .Returns(plannedTransaction);
        _customPlannedTransactionRepository
            .AnyByPlannedTransactionIdAsync(query.PlannedTransactionId)
            .Returns(true);

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        result
            .Should()
            .BeEquivalentTo(plannedTransaction,
                opt => opt.ExcludingMissingMembers().Excluding(e => e.Description).Excluding(e => e.Amount)
                    .Excluding(e => e.StartsAt)
                    .Excluding(e => e.EndsAt));
        result!.Description.Should().Be(custom.Description);
        result.Amount.Should().Be(custom.Amount);
        result.StartsAt.Should().Be(custom.RefersTo);
        result.EndsAt.Should().Be(custom.RefersTo);
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(default, Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_should_return_the_planned_transaction()
    {
        // Arrange
        var plannedTransaction = new PlannedTransactionBuilder()
            .WithoutCustomPlannedTransactions()
            .Build();
        var query = Fixture.Build<GetPlannedTransactionByIdAndDateQuery>()
            .With(w => w.PlannedTransactionId, plannedTransaction.Id)
            .Create();

        _plannedTransactionRepository
            .GetByIdAndDateWithCustomPlannedAsync(query.PlannedTransactionId,
                Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month))
            .Returns(plannedTransaction);
        _customPlannedTransactionRepository
            .AnyByPlannedTransactionIdAsync(query.PlannedTransactionId)
            .Returns(false);
        
        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .NotBeNull();
        result
            .Should()
            .BeEquivalentTo(plannedTransaction,
                opt => opt.ExcludingMissingMembers());
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(default, Arg.Any<string>());
    }
}