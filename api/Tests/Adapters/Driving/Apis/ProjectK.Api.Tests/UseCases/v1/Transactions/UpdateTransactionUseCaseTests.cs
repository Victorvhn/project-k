using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.Transactions.Requests;
using ProjectK.Api.UseCases.v1.Transactions;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;
using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Core.Entities;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Transactions;

public class UpdateTransactionUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ISender _sender;

    private readonly IUpdateTransactionUseCase _updateTransactionUseCase;

    public UpdateTransactionUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _updateTransactionUseCase = new UpdateTransactionUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_update_a_transaction()
    {
        // Arrange
        var transactionId = Fixture.Create<Ulid>();
        var request = Fixture.Create<SaveTransactionRequest>();
        var plannedTransaction = Fixture.Create<Transaction>();

        _sender
            .Send(Arg.Is<UpdateTransactionCommand>(a =>
                a.TransactionId == transactionId &&
                a.Description == request.Description &&
                a.Amount == request.Amount &&
                a.Type == request.Type &&
                a.PaidAt == request.PaidAt &&
                a.CategoryId == request.CategoryId &&
                a.PlannedTransactionId == request.PlannedTransactionId
            ))
            .Returns(plannedTransaction);

        // Act
        var result = await _updateTransactionUseCase.ExecuteAsync(transactionId, request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(plannedTransaction, opt => opt.ExcludingMissingMembers());
    }
}