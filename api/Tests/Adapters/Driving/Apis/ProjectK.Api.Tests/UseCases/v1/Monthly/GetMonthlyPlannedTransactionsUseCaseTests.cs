using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.UseCases.v1.Monthly;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Queries.v1.PlannedTransactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Monthly;

public class GetMonthlyPlannedTransactionsUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly IGetMonthlyPlannedTransactionsUseCase _getMonthlyPlannedTransactionsUseCase;
    private readonly ISender _sender;

    public GetMonthlyPlannedTransactionsUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getMonthlyPlannedTransactionsUseCase =
            new GetMonthlyPlannedTransactionsUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_return_planned_transactions()
    {
        // Arrange
        var request = Fixture.Create<MonthlyRequest>();
        var plannedTransactions = Fixture.CreateMany<MonthlyPlannedTransactionDto>(5).ToList();

        _sender
            .Send(Arg.Is<GetMonthlyPlannedTransactionsQuery>(a =>
                a.Month == request.Month &&
                a.Year == request.Year
            ))
            .Returns(plannedTransactions);

        // Act
        var result = await _getMonthlyPlannedTransactionsUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(plannedTransactions, opt => opt.ExcludingMissingMembers());
    }
}