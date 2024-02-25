using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ProjectK.Api.Controllers.v1;
using ProjectK.Api.Dtos.v1.Transactions.Requests;
using ProjectK.Api.Dtos.v1.Transactions.Responses;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.Controllers.v1;

public class TransactionsControllerTests
{
    private readonly ICreateTransactionUseCase _createTransactionUseCase;
    private readonly IDeleteTransactionUseCase _deleteTransactionUseCase;
    private readonly Fixture _fixture = CustomAutoFixture.Create();
    private readonly IGetTransactionUseCase _getTransactionUseCase;

    private readonly TransactionsController _transactionsController;
    private readonly IUpdateTransactionUseCase _updateTransactionUseCase;


    public TransactionsControllerTests()
    {
        _createTransactionUseCase = Substitute.For<ICreateTransactionUseCase>();
        _updateTransactionUseCase = Substitute.For<IUpdateTransactionUseCase>();
        _deleteTransactionUseCase = Substitute.For<IDeleteTransactionUseCase>();
        _getTransactionUseCase = Substitute.For<IGetTransactionUseCase>();

        _transactionsController = new TransactionsController(_createTransactionUseCase, _updateTransactionUseCase,
            _deleteTransactionUseCase, _getTransactionUseCase);
    }

    [Fact]
    public async Task Create_Should_Return_Created()
    {
        var request = _fixture.Create<SaveTransactionRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<TransactionDto>();

        _createTransactionUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _transactionsController.Create(request, cancellationToken);

        var result = actionResult.Result as CreatedResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }

    [Fact]
    public async Task Update_Should_Return_Ok()
    {
        var transactionId = _fixture.Create<Ulid>();
        var request = _fixture.Create<SaveTransactionRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<TransactionDto>();

        _updateTransactionUseCase
            .ExecuteAsync(transactionId, request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _transactionsController.Update(transactionId, request, cancellationToken);

        var result = actionResult.Result as OkObjectResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }

    [Fact]
    public async Task Delete_should_return_NoContent()
    {
        var transactionId = _fixture.Create<Ulid>();
        var cancellationToken = new CancellationToken();

        var actionResult = await _transactionsController.Delete(transactionId, cancellationToken);

        var result = actionResult as NoContentResult;
        result
            .Should()
            .NotBeNull();
        await _deleteTransactionUseCase
            .Received(1)
            .ExecuteAsync(transactionId, cancellationToken);
    }

    [Fact]
    public async Task GetById_should_return_Ok()
    {
        var transactionId = _fixture.Create<Ulid>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<TransactionDto>();

        _getTransactionUseCase
            .ExecuteAsync(transactionId, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _transactionsController.GetById(transactionId, cancellationToken);

        var result = actionResult.Result as OkObjectResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }
}