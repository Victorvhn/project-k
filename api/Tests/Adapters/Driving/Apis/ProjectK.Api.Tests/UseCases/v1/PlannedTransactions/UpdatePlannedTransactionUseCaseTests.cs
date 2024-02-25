using AutoFixture;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Commands.v1.PlannedTransactions.Update;
using ProjectK.Core.Enums;
using ProjectK.Core.Infrastructure.Notifications;
using ProjectK.Core.Infrastructure.Notifications.Infrastructure;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.PlannedTransactions;

public class UpdatePlannedTransactionUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly INotificationManager _notificationManager;
    private readonly ISender _sender;
    private readonly IUpdatePlannedTransactionUseCase _updatePlannedTransactionUseCase;

    public UpdatePlannedTransactionUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _notificationManager = Substitute.For<INotificationManager>();
        _sender = Substitute.For<ISender>();

        _updatePlannedTransactionUseCase = new UpdatePlannedTransactionUseCase(mapper, _notificationManager, _sender);
    }

    [Theory]
    [InlineData(ActionType.All, typeof(UpdatePlannedTransactionCommand))]
    [InlineData(ActionType.FromNowOn, typeof(UpdateFromNowOnPlannedTransactionCommand))]
    [InlineData(ActionType.JustOne, typeof(UpdateMonthlyPlannedTransactionCommand))]
    public async Task It_should_delete_a_planned_transaction(ActionType actionType, Type commandType)
    {
        // Arrange
        var plannedTransactionId = Fixture.Create<Ulid>();
        var request = Fixture.Build<UpdatePlannedTransactionRequest>()
            .With(w => w.ActionType, actionType)
            .Create();

        // Act
        await _updatePlannedTransactionUseCase.ExecuteAsync(plannedTransactionId, request);

        // Assert
        await _sender
            .Received(1)
            .Send(Arg.Is<UpdatePlannedTransactionCommandBase>(a =>
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
        var request = Fixture.Build<UpdatePlannedTransactionRequest>()
            .With(w => w.ActionType, (ActionType)100)
            .Create();

        // Act
        await _updatePlannedTransactionUseCase.ExecuteAsync(plannedTransactionId, request);

        // Assert
        _notificationManager
            .Received(1)
            .Add(NotificationType.BadRequest, Resources.InvalidActionTypeProvided);
        await _sender
            .DidNotReceiveWithAnyArgs()
            .Send(Arg.Any<UpdatePlannedTransactionCommandBase>());
    }
}