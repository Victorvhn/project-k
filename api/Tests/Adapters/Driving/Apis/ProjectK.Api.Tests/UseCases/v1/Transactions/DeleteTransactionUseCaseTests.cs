using AutoFixture;
using Mediator;
using NSubstitute;
using ProjectK.Api.UseCases.v1.Transactions;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;
using ProjectK.Core.Commands.v1.Transactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Transactions;

public class DeleteTransactionUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly IDeleteTransactionUseCase _deleteTransactionUseCase;
    private readonly ISender _sender;

    public DeleteTransactionUseCaseTests()
    {
        _sender = Substitute.For<ISender>();

        _deleteTransactionUseCase = new DeleteTransactionUseCase(_sender);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Call_TransactionService()
    {
        var transactionId = Fixture.Create<Ulid>();

        await _deleteTransactionUseCase.ExecuteAsync(transactionId);

        await _sender
            .Received(1)
            .Send(Arg.Is<DeleteTransactionCommand>(a => a.TransactionId == transactionId));
    }
}