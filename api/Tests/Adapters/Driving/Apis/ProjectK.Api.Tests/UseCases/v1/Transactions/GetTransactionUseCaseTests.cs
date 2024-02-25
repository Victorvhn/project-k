using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.UseCases.v1.Transactions;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.Transactions;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Transactions;

public class GetTransactionUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly IGetTransactionUseCase _getTransactionUseCase;
    private readonly ISender _sender;

    public GetTransactionUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getTransactionUseCase = new GetTransactionUseCase(mapper, _sender);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Call_TransactionService()
    {
        var transactionId = Fixture.Create<Ulid>();
        var cancellationToken = new CancellationToken();
        var transaction = Fixture.Create<Transaction>();

        _sender
            .Send(Arg.Is<GetTransactionByIdQuery>(a => a.TransactionId == transactionId), cancellationToken)
            .Returns(transaction);

        var result = await _getTransactionUseCase.ExecuteAsync(transactionId, cancellationToken);

        result
            .Should()
            .BeEquivalentTo(transaction, opt => opt.ExcludingMissingMembers());
    }
}