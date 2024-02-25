using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.UseCases.v1.Monthly;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Enums;
using ProjectK.Core.Queries.v1.Summary;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Monthly;

public class GetMonthlySummaryUseCaseTests
{
    private readonly Fixture _fixture = CustomAutoFixture.Create();
    private readonly IGetMonthlySummaryUseCase _getMonthlySummaryUseCase;
    private readonly ISender _sender;

    public GetMonthlySummaryUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getMonthlySummaryUseCase =
            new GetMonthlySummaryUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_return_summary()
    {
        // Arrange
        var request = _fixture.Create<MonthlyRequest>();
        var summary = _fixture.Create<SummaryDto>();

        _sender
            .Send(Arg.Is<GetMonthlySummaryQuery>(a =>
                a.TransactionType == TransactionType.Expense &&
                a.Month == request.Month &&
                a.Year == request.Year
            ))
            .Returns(summary);

        // Act
        var result = await _getMonthlySummaryUseCase.ExecuteAsync(request, TransactionType.Expense);

        // Assert
        result
            .Should()
            .BeEquivalentTo(summary, opt => opt.ExcludingMissingMembers());
    }
}