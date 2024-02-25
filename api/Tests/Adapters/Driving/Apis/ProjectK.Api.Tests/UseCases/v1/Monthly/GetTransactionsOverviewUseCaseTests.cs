using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.UseCases.v1.Monthly;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Queries.v1.Summary;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Monthly;

public class GetTransactionsOverviewUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly IGetTransactionsOverviewUseCase _getTransactionsOverviewUseCase;
    private readonly ISender _sender;

    public GetTransactionsOverviewUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getTransactionsOverviewUseCase =
            new GetTransactionsOverviewUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_return_expenses_overview()
    {
        // Arrange
        var request = Fixture.Create<MonthlyRequest>();
        var expensesOverview = Fixture.CreateMany<MonthlyExpensesOverviewData>(5).ToList();

        _sender
            .Send(Arg.Is<GetMonthlyOverviewByCategoryQuery>(a =>
                a.Month == request.Month &&
                a.Year == request.Year
            ))
            .Returns(expensesOverview);

        // Act
        var result = await _getTransactionsOverviewUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(expensesOverview, opt => opt.ExcludingMissingMembers());
    }
}