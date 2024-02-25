using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Commands.v1.PlannedTransactions;
using ProjectK.Core.Entities;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.PlannedTransactions;

public class CreatePlannedTransactionUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly ICreatePlannedTransactionUseCase _createCategoryUseCase;

    private readonly ISender _sender;

    public CreatePlannedTransactionUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _createCategoryUseCase = new CreatePlannedTransactionUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_create_a_planned_transaction()
    {
        // Arrange
        var request = Fixture.Create<CreatePlannedTransactionRequest>();
        var plannedTransaction = Fixture.Create<PlannedTransaction>();

        _sender
            .Send(Arg.Is<CreatePlannedTransactionCommand>(a =>
                a.Description == request.Description &&
                a.Amount == request.Amount &&
                a.AmountType == request.AmountType &&
                a.Type == request.Type &&
                a.Recurrence == request.Recurrence &&
                a.StartsAt == request.StartsAt &&
                a.EndsAt == request.EndsAt &&
                a.CategoryId == request.CategoryId
            ))
            .Returns(plannedTransaction);

        // Act
        var result = await _createCategoryUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(plannedTransaction, opt => opt.ExcludingMissingMembers());
    }
}