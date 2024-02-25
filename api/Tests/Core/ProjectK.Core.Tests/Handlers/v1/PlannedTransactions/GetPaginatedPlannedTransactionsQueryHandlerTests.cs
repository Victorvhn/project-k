using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Handlers.v1.PlannedTransactions;
using ProjectK.Core.Queries.v1.PlannedTransactions;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.PlannedTransactions;

public class GetPaginatedPlannedTransactionsQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly GetPaginatedPlannedTransactionsQueryHandler _commandHandler;

    private readonly IPlannedTransactionRepository _plannedTransactionRepository;

    public GetPaginatedPlannedTransactionsQueryHandlerTests()
    {
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();

        _commandHandler = new GetPaginatedPlannedTransactionsQueryHandler(_plannedTransactionRepository);
    }

    [Fact]
    public async Task Handle_should_return_paginated_planned_transactions()
    {
        // Arrange
        var query = Fixture.Create<GetPaginatedPlannedTransactionsQuery>();
        var plannedTransactions = PlannedTransactionBuilder.CreateMany(5);

        _plannedTransactionRepository
            .GetPaginatedAsync(Arg.Is<PaginationFilter>(a =>
                a.CurrentPage == query.CurrentPage && a.PageSize == query.PageSize))
            .Returns((plannedTransactions, 5));

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Data
            .Should()
            .BeEquivalentTo(plannedTransactions);
        result
            .TotalCount
            .Should()
            .Be(5);
        result
            .CurrentPage
            .Should()
            .Be(query.CurrentPage);
        result
            .PageSize
            .Should()
            .Be(query.PageSize);
    }
}