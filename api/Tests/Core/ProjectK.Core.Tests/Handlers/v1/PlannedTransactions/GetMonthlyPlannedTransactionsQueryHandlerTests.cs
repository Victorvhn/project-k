using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Handlers.v1.PlannedTransactions;
using ProjectK.Core.Queries.v1.PlannedTransactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Handlers.v1.PlannedTransactions;

public class GetMonthlyPlannedTransactionsQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly GetMonthlyPlannedTransactionsQueryHandler _commandHandler;

    private readonly IPlannedTransactionRepository _plannedTransactionRepository;

    public GetMonthlyPlannedTransactionsQueryHandlerTests()
    {
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();

        _commandHandler = new GetMonthlyPlannedTransactionsQueryHandler(_plannedTransactionRepository);
    }

    [Fact]
    public async Task Handle_should_return_planned_transactions()
    {
        // Arrange
        var query = Fixture.Create<GetMonthlyPlannedTransactionsQuery>();
        var plannedTransactions = Fixture.CreateMany<MonthlyPlannedTransactionDto>(5)
            .ToList();

        _plannedTransactionRepository
            .GetMonthlyAsync(Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month))
            .Returns(plannedTransactions);

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(plannedTransactions, opt => opt.ExcludingMissingMembers());
    }
}