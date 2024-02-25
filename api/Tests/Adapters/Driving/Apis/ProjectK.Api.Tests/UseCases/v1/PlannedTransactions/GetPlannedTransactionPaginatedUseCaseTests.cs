using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.UseCases.v1.PlannedTransactions;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.PlannedTransactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.PlannedTransactions;

public class GetPlannedTransactionPaginatedUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly IGetPlannedTransactionPaginatedUseCase _getPlannedTransactionPaginatedUseCase;

    private readonly ISender _sender;

    public GetPlannedTransactionPaginatedUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getPlannedTransactionPaginatedUseCase =
            new GetPlannedTransactionPaginatedUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_get_paginated_planned_transactions()
    {
        // Arrange
        var paginatedRequest = Fixture.Create<PaginatedRequest>();
        var paginatedResponse = Fixture.Create<PaginatedData<PlannedTransaction>>();

        _sender
            .Send(Arg.Is<GetPaginatedPlannedTransactionsQuery>(a =>
                a.CurrentPage == paginatedRequest.CurrentPage &&
                a.PageSize == paginatedRequest.PageSize
            ))
            .Returns(paginatedResponse);

        // Act
        var result =
            await _getPlannedTransactionPaginatedUseCase.ExecuteAsync(paginatedRequest);

        // Assert
        result
            .Should()
            .BeEquivalentTo(paginatedResponse, opt => opt.ExcludingMissingMembers());
    }
}