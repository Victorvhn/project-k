using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.UseCases.v1.Monthly;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.Transactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Monthly;

public class GetMonthlyTransactionsUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly IGetMonthlyTransactionsUseCase _getMonthlyTransactionsUseCase;
    private readonly ISender _sender;

    public GetMonthlyTransactionsUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getMonthlyTransactionsUseCase =
            new GetMonthlyTransactionsUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_return_transactions()
    {
        // Arrange
        var request = Fixture.Create<MonthlyRequest>();
        var transactions = Fixture.CreateMany<Transaction>(5).ToList();

        _sender
            .Send(Arg.Is<GetMonthlyTransactionsQuery>(a =>
                a.Month == request.Month &&
                a.Year == request.Year
            ))
            .Returns(transactions);

        // Act
        var result = await _getMonthlyTransactionsUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(transactions, opt => opt.ExcludingMissingMembers());
    }
}