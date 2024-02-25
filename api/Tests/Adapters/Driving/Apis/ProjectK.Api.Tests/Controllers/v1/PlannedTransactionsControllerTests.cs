using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ProjectK.Api.Controllers.v1;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.Controllers.v1;

public class PlannedTransactionsControllerTests
{
    private readonly ICreatePlannedTransactionUseCase _createPlannedTransactionUseCase;
    private readonly IDeletePlannedTransactionUseCase _deletePlannedTransactionUseCase;
    private readonly Fixture _fixture = CustomAutoFixture.Create();
    private readonly IGetPlannedTransactionPaginatedUseCase _getPlannedTransactionPaginatedUseCase;

    private readonly PlannedTransactionsController _plannedTransactionsController;
    private readonly IUpdatePlannedTransactionUseCase _updatePlannedTransactionUseCase;

    public PlannedTransactionsControllerTests()
    {
        _getPlannedTransactionPaginatedUseCase = Substitute.For<IGetPlannedTransactionPaginatedUseCase>();
        _createPlannedTransactionUseCase = Substitute.For<ICreatePlannedTransactionUseCase>();
        _updatePlannedTransactionUseCase = Substitute.For<IUpdatePlannedTransactionUseCase>();
        _deletePlannedTransactionUseCase = Substitute.For<IDeletePlannedTransactionUseCase>();

        _plannedTransactionsController =
            new PlannedTransactionsController(_getPlannedTransactionPaginatedUseCase, _createPlannedTransactionUseCase,
                _updatePlannedTransactionUseCase,
                _deletePlannedTransactionUseCase);
    }

    [Fact]
    public async Task Get_Should_Return_Ok()
    {
        var request = _fixture.Create<PaginatedRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<PaginatedResponse<PlannedTransactionDto>>();

        _getPlannedTransactionPaginatedUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _plannedTransactionsController.Get(request, cancellationToken);

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
    public async Task Create_Should_Return_Created()
    {
        var request = _fixture.Create<CreatePlannedTransactionRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<PlannedTransactionDto>();

        _createPlannedTransactionUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _plannedTransactionsController.Create(request, cancellationToken);

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
        var id = _fixture.Create<Ulid>();
        var request = _fixture.Create<UpdatePlannedTransactionRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<PlannedTransactionDto>();

        _updatePlannedTransactionUseCase
            .ExecuteAsync(id, request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult =
            await _plannedTransactionsController.Update(id, request, cancellationToken);

        var result = actionResult as OkObjectResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }

    [Fact]
    public async Task Delete_Should_Return_NoContent()
    {
        var id = _fixture.Create<Ulid>();
        var request = _fixture.Create<DeletePlannedTransactionRequest>();
        var cancellationToken = new CancellationToken();

        var actionResult =
            await _plannedTransactionsController.Delete(id, request, cancellationToken);

        var result = actionResult as NoContentResult;
        result
            .Should()
            .NotBeNull();
        await _deletePlannedTransactionUseCase
            .Received(1)
            .ExecuteAsync(id, request, cancellationToken);
    }
}