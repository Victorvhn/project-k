using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Transactions;
using ProjectK.Core.Queries.v1.Transactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Handlers.v1.Transactions;

public class GetMonthlyTransactionsQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly GetMonthlyTransactionsQueryHandler _commandHandler;

    private readonly ITransactionRepository _transactionRepository;

    public GetMonthlyTransactionsQueryHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionRepository>();

        _commandHandler = new GetMonthlyTransactionsQueryHandler(_transactionRepository);
    }

    [Fact]
    public async Task Handle_should_return_transactions()
    {
        // Arrange
        var query = Fixture.Create<GetMonthlyTransactionsQuery>();
        var transactions = Fixture.CreateMany<Transaction>(5).ToList();

        _transactionRepository
            .GetMonthlyAsync(Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month))
            .Returns(transactions);

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(transactions, opt => opt.ExcludingMissingMembers());
    }
}