using AutoFixture;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Commands.v1.PlannedTransactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.PlannedTransactions;

public class PayPlannedTransactionUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly IPayPlannedTransactionUseCase _payPlannedTransactionUseCase;
    private readonly ISender _sender;

    public PayPlannedTransactionUseCaseTests()
    {
        _sender = Substitute.For<ISender>();

        _payPlannedTransactionUseCase =
            new PayPlannedTransactionUseCase(_sender);
    }

    [Fact]
    public async Task It_should_pay_a_planned_transaction()
    {
        // Arrange
        var plannedTransactionId = Fixture.Create<Ulid>();
        var monthlyRequest = Fixture.Create<MonthlyRequest>();
        var request = Fixture.Create<PayPlannedTransactionRequest>();

        // Act
        await _payPlannedTransactionUseCase.ExecuteAsync(plannedTransactionId, monthlyRequest, request);

        // Assert
        await _sender
            .Received(1)
            .Send(Arg.Is<PayPlannedTransactionCommand>(a =>
                a.PlannedTransactionId == plannedTransactionId &&
                a.Year == monthlyRequest.Year &&
                a.Month == monthlyRequest.Month &&
                a.Amount == request.Amount
            ));
    }
}