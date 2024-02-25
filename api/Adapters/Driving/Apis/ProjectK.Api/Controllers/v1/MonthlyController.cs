using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProjectK.Api.Attributes;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;
using ProjectK.Api.Dtos.v1.Transactions.Responses;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Enums;

// ReSharper disable RouteTemplates.ControllerRouteParameterIsNotPassedToMethods
// ReSharper disable RouteTemplates.MethodMissingRouteParameters

namespace ProjectK.Api.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]/{year:int}/{month:int}")]
public class MonthlyController(
    IGetPlannedTransactionForDateUseCase getPlannedTransactionForDateUseCase,
    IGetMonthlyPlannedTransactionsUseCase getMonthlyPlannedTransactionsUseCase,
    IGetMonthlySummaryUseCase getMonthlySummaryUseCase,
    IGetMonthlyTransactionsUseCase getMonthlyTransactionsUseCase,
    IPayPlannedTransactionUseCase payPlannedTransactionUseCase,
    IGetTransactionsOverviewUseCase getTransactionsOverviewUseCase) : ApiControllerBase
{
    /// <summary>
    ///     Retrieves a planned transaction by ID for a specific date.
    /// </summary>
    /// <param name="plannedTransactionId">The ID of the planned transaction to retrieve.</param>
    /// <param name="monthlyRequest">The monthly request containing specific date information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the planned transaction.</returns>
    [HttpGet("{plannedTransactionId}")]
    [ProducesResponseType(typeof(PlannedTransactionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PlannedTransactionDto>> GetByIdForDate([FromRoute] Ulid plannedTransactionId,
        [FromRoute] MonthlyRequest monthlyRequest,
        CancellationToken cancellationToken)
    {
        var result =
            await getPlannedTransactionForDateUseCase.ExecuteAsync(plannedTransactionId, monthlyRequest,
                cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Retrieves planned transactions for a specific date.
    /// </summary>
    /// <param name="request">The request containing the year and month for data retrieval.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns a list of planned transactions for the specified month.</returns>
    [HttpGet("planned-transactions")]
    [ProducesResponseType(typeof(IEnumerable<MonthlyPlannedTransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MonthlyPlannedTransactionDto>>> GetPlannedTransactions(
        [FromRoute] MonthlyRequest request, CancellationToken cancellationToken)
    {
        var result = await getMonthlyPlannedTransactionsUseCase.ExecuteAsync(request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Retrieves a summary of expected and current values for a specific date.
    /// </summary>
    /// <param name="request">The request containing the year and month for data retrieval.</param>
    /// <param name="transactionType">The Transaction Type for data retrieval.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns a summary of planned and expended values for the specified month.</returns>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(SummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SummaryDto>> GetSummary(
        [FromRoute] MonthlyRequest request,
        [FromQuery] TransactionType transactionType,
        CancellationToken cancellationToken)
    {
        var result = await getMonthlySummaryUseCase.ExecuteAsync(request, transactionType, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Retrieves transactions for a specific date.
    /// </summary>
    /// <param name="request">The request containing the year and month for data retrieval.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns a list of transactions for the specified month.</returns>
    [HttpGet("transactions")]
    [ProducesResponseType(typeof(IEnumerable<MonthlyTransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MonthlyTransactionDto>>> GetTransactions(
        [FromRoute] MonthlyRequest request, CancellationToken cancellationToken)
    {
        var result = await getMonthlyTransactionsUseCase.ExecuteAsync(request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Pays a planned transactions for a specific date.
    /// </summary>
    /// <param name="plannedTransactionId">The ID of the planned transaction to be used.</param>
    /// <param name="monthlyRequest">The request containing the year and month for data creation.</param>
    /// <param name="request">The request containing the data for transaction payment creation.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns an <see cref="ActionResult" /> indicating the result of the update operation.</returns>
    [Transaction]
    [HttpPost("planned-transaction/{plannedTransactionId}/pay")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> PayPlannedTransaction(
        [FromRoute] Ulid plannedTransactionId, [FromRoute] MonthlyRequest monthlyRequest,
        [FromBody] PayPlannedTransactionRequest request, CancellationToken cancellationToken)
    {
        await payPlannedTransactionUseCase.ExecuteAsync(plannedTransactionId, monthlyRequest, request, cancellationToken);

        return NoContent();
    }

    /// <summary>
    ///     Retrieves an overview of planned and expended resources for a specified month.
    /// </summary>
    /// <param name="request">The request object containing the year and month for data retrieval.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if necessary.</param>
    /// <returns>Returns a list of planned and expended resources grouped by categories for the specified month.</returns>
    [HttpGet("expenses-overview")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IEnumerable<MonthlyExpensesOverviewResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MonthlyExpensesOverviewResponse>>> GetTransactionsOverview(
        [FromRoute] MonthlyRequest request, CancellationToken cancellationToken)
    {
        var result = await getTransactionsOverviewUseCase.ExecuteAsync(request, cancellationToken);

        if (!result.Any()) return NoContent();

        return Ok(result);
    }
}