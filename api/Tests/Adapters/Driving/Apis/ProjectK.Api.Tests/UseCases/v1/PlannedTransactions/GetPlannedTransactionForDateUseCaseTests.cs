using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Queries.v1.PlannedTransactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.PlannedTransactions;

public class GetPlannedTransactionForDateUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly IGetPlannedTransactionForDateUseCase _getPlannedTransactionForDateUseCase;
    private readonly ISender _sender;

    public GetPlannedTransactionForDateUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getPlannedTransactionForDateUseCase =
            new GetPlannedTransactionForDateUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_get_a_planned_transaction_by_id_for_date()
    {
        // Arrange
        var plannedTransactionId = Fixture.Create<Ulid>();
        var monthlyRequest = Fixture.Create<MonthlyRequest>();
        var plannedTransaction = Fixture.Create<PlannedTransactionDto>();

        _sender
            .Send(Arg.Is<GetPlannedTransactionByIdAndDateQuery>(a =>
                a.PlannedTransactionId == plannedTransactionId &&
                a.Year == monthlyRequest.Year &&
                a.Month == monthlyRequest.Month
            ))
            .Returns(plannedTransaction);

        // Act
        var result =
            await _getPlannedTransactionForDateUseCase.ExecuteAsync(plannedTransactionId, monthlyRequest);

        // Assert
        result
            .Should()
            .BeEquivalentTo(plannedTransaction, opt => opt.ExcludingMissingMembers());
    }
}