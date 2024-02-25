using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Handlers.v1.Summary;
using ProjectK.Core.Queries.v1.Summary;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Handlers.v1.Summary;

public class GetMonthlySummaryQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly GetMonthlySummaryQueryHandler _commandHandler;
    private readonly IPlannedTransactionRepository _plannedTransactionRepository;

    private readonly ITransactionRepository _transactionRepository;

    public GetMonthlySummaryQueryHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();

        _commandHandler = new GetMonthlySummaryQueryHandler(_transactionRepository,
            _plannedTransactionRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSummaryDto()
    {
        // Arrange
        var query = Fixture.Create<GetMonthlySummaryQuery>();

        var expectedAmount = Fixture.Create<decimal>();
        var actualAmount = Fixture.Create<decimal>();

        _plannedTransactionRepository
            .GetExpectedAmountSummaryByDateAsync(
                Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month), query.TransactionType)
            .Returns(expectedAmount);

        _transactionRepository
            .GetCurrentAmountSummaryByDateAsync(
                Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month), query.TransactionType)
            .Returns(actualAmount);

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(new SummaryDto(expectedAmount, actualAmount));
    }
}