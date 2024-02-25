using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ProjectK.Api.Controllers.v1;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;
using ProjectK.Api.Dtos.v1.Transactions.Responses;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Enums;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.Controllers.v1;

public class MonthlyControllerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly IGetMonthlyPlannedTransactionsUseCase _getMonthlyPlannedTransactionsUseCase;
    private readonly IGetMonthlySummaryUseCase _getMonthlySummaryUseCase;
    private readonly IGetMonthlyTransactionsUseCase _getMonthlyTransactionsUseCase;
    private readonly IGetPlannedTransactionForDateUseCase _getPlannedTransactionForDateUseCase;
    private readonly IGetTransactionsOverviewUseCase _getTransactionsOverviewUseCase;

    private readonly MonthlyController _monthlyController;
    private readonly IPayPlannedTransactionUseCase _payPlannedTransactionUseCase;

    public MonthlyControllerTests()
    {
        _getPlannedTransactionForDateUseCase = Substitute.For<IGetPlannedTransactionForDateUseCase>();
        _getMonthlyPlannedTransactionsUseCase = Substitute.For<IGetMonthlyPlannedTransactionsUseCase>();
        _getMonthlySummaryUseCase = Substitute.For<IGetMonthlySummaryUseCase>();
        _getMonthlyTransactionsUseCase = Substitute.For<IGetMonthlyTransactionsUseCase>();
        _payPlannedTransactionUseCase = Substitute.For<IPayPlannedTransactionUseCase>();
        _getTransactionsOverviewUseCase = Substitute.For<IGetTransactionsOverviewUseCase>();

        _monthlyController = new MonthlyController(_getPlannedTransactionForDateUseCase,
            _getMonthlyPlannedTransactionsUseCase, _getMonthlySummaryUseCase, _getMonthlyTransactionsUseCase,
            _payPlannedTransactionUseCase, _getTransactionsOverviewUseCase);
    }

    [Fact]
    public async Task GetByIdForDate_Should_Return_Ok()
    {
        var id = Fixture.Create<Ulid>();
        var request = Fixture.Create<MonthlyRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = Fixture.Create<PlannedTransactionDto>();

        _getPlannedTransactionForDateUseCase
            .ExecuteAsync(id, request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _monthlyController.GetByIdForDate(id, request, cancellationToken);

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
    public async Task GetPlannedTransactions_Should_Return_Ok()
    {
        var request = Fixture.Create<MonthlyRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = Fixture.CreateMany<MonthlyPlannedTransactionDto>(5).ToList();

        _getMonthlyPlannedTransactionsUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _monthlyController.GetPlannedTransactions(request, cancellationToken);

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
    public async Task GetSummary_Should_Return_Ok()
    {
        var request = Fixture.Create<MonthlyRequest>();
        const TransactionType transactionType = TransactionType.Expense;
        var cancellationToken = Fixture.Create<CancellationToken>();
        var useCaseResult = Fixture.Create<SummaryDto>();

        _getMonthlySummaryUseCase
            .ExecuteAsync(request, transactionType, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _monthlyController.GetSummary(request, transactionType, cancellationToken);

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
    public async Task GetTransactions_Should_Return_Ok()
    {
        var request = Fixture.Create<MonthlyRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = Fixture.CreateMany<MonthlyTransactionDto>(5).ToList();

        _getMonthlyTransactionsUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _monthlyController.GetTransactions(request, cancellationToken);

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
    public async Task PayPlannedTransaction_Should_Return_NoContent()
    {
        var id = Fixture.Create<Ulid>();
        var monthlyRequest = Fixture.Create<MonthlyRequest>();
        var request = Fixture.Create<PayPlannedTransactionRequest>();
        var cancellationToken = new CancellationToken();

        var actionResult = await _monthlyController.PayPlannedTransaction(id, monthlyRequest, request, cancellationToken);

        var result = actionResult as NoContentResult;
        result
            .Should()
            .NotBeNull();
        await _payPlannedTransactionUseCase
            .Received(1)
            .ExecuteAsync(id, monthlyRequest, request, cancellationToken);
    }

    [Fact]
    public async Task GetExpensesOverview_Should_Return_Ok()
    {
        var request = Fixture.Create<MonthlyRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = Fixture.CreateMany<MonthlyExpensesOverviewResponse>(5).ToList();

        _getTransactionsOverviewUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _monthlyController.GetTransactionsOverview(request, cancellationToken);

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
    public async Task GetExpensesOverview_Should_Return_NoContent()
    {
        var request = Fixture.Create<MonthlyRequest>();
        var cancellationToken = new CancellationToken();

        _getTransactionsOverviewUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(ArraySegment<MonthlyExpensesOverviewResponse>.Empty);

        var actionResult = await _monthlyController.GetTransactionsOverview(request, cancellationToken);

        var result = actionResult.Result as NoContentResult;
        result
            .Should()
            .NotBeNull();
    }
}