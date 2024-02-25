using AutoFixture;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions;
using ProjectK.Core.Commands.v1.PlannedTransactions.Delete;
using ProjectK.Core.Enums;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.PlannedTransactions;

public class DeletePlannedTransactionUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly DeletePlannedTransactionUseCase _deletePlannedTransactionUseCase;
    private readonly INotificationManager _notificationManager;
    private readonly ISender _sender;

    public DeletePlannedTransactionUseCaseTests()
    {
        _notificationManager = Substitute.For<INotificationManager>();
        _sender = Substitute.For<ISender>();

        _deletePlannedTransactionUseCase =
            new DeletePlannedTransactionUseCase(_notificationManager, _sender);
    }

    [Theory]
    [InlineData(ActionType.All, typeof(DeletePlannedTransactionCommand))]
    [InlineData(ActionType.FromNowOn, typeof(DeleteFromNowOnPlannedTransaction))]
    [InlineData(ActionType.JustOne, typeof(DeleteMonthlyPlannedTransactionCommand))]
    public async Task It_should_delete_a_planned_transaction(ActionType actionType, Type commandType)
    {
        // Arrange
        var plannedTransactionId = Fixture.Create<Ulid>();
        var request = Fixture.Build<DeletePlannedTransactionRequest>()
            .With(w => w.ActionType, actionType)
            .Create();

        // Act
        await _deletePlannedTransactionUseCase.ExecuteAsync(plannedTransactionId, request);

        // Assert
        await _sender
            .Received(1)
            .Send(Arg.Is<DeletePlannedTransactionCommandBase>(a =>
                a.GetType() == commandType &&
                a.PlannedTransactionId == plannedTransactionId));
        _notificationManager
            .DidNotReceiveWithAnyArgs()
            .Add(Arg.Any<NotificationType>(), Arg.Any<string>());
    }

    [Fact]
    public async Task It_should_add_an_invalid_action_type_notification_when_action_type_is_invalid()
    {
        // Arrange
        var plannedTransactionId = Fixture.Create<Ulid>();
        var request = Fixture.Build<DeletePlannedTransactionRequest>()
            .With(w => w.ActionType, (ActionType)100)
            .Create();

        // Act
        await _deletePlannedTransactionUseCase.ExecuteAsync(plannedTransactionId, request);

        // Assert
        _notificationManager
            .Received(1)
            .Add(NotificationType.BadRequest, Resources.InvalidActionTypeProvided);
        await _sender
            .DidNotReceiveWithAnyArgs()
            .Send(Arg.Any<DeletePlannedTransactionCommandBase>());
    }
}