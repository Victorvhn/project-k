using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProjectK.Api.Attributes;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;

namespace ProjectK.Api.Controllers.v1;

[ApiVersion("1")]
public class PlannedTransactionsController(
    IGetPlannedTransactionPaginatedUseCase getPlannedTransactionPaginatedUseCase,
    ICreatePlannedTransactionUseCase createPlannedTransactionUseCase,
    IUpdatePlannedTransactionUseCase updatePlannedTransactionUseCase,
    IDeletePlannedTransactionUseCase deletePlannedTransactionUseCase)
    : ApiControllerBase
{
    /// <summary>
    ///     Retrieves a paginated list of planned transactions.
    /// </summary>
    /// <param name="request">Pagination parameters.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>The paginated list of planned transactions.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<PlannedTransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<PlannedTransactionDto>>> Get([FromQuery] PaginatedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await getPlannedTransactionPaginatedUseCase.ExecuteAsync(request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Creates a planned transaction.
    /// </summary>
    /// <param name="request">The planned transaction creation request.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns the created planned transaction.</returns>
    [HttpPost]
    [Transaction]
    [ProducesResponseType(typeof(PlannedTransactionDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<PlannedTransactionDto>> Create([FromBody] CreatePlannedTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await createPlannedTransactionUseCase.ExecuteAsync(request, cancellationToken);

        return Created("/", result);
    }

    /// <summary>
    ///     Updates a planned transaction.
    /// </summary>
    /// <param name="plannedTransactionId">The ID of the planned transaction to be updated.</param>
    /// <param name="request">The request body containing updated details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns an <see cref="ActionResult" /> indicating the result of the update operation.</returns>
    [HttpPut("{plannedTransactionId}")]
    [Transaction]
    [ProducesResponseType(typeof(PlannedTransactionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> Update([FromRoute] Ulid plannedTransactionId,
        [FromBody] UpdatePlannedTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var result =
            await updatePlannedTransactionUseCase.ExecuteAsync(plannedTransactionId, request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Deletes a planned transaction.
    /// </summary>
    /// <param name="plannedTransactionId">The ID of the planned transaction to be deleted.</param>
    /// <param name="request">The request containing specific date and action.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns an <see cref="ActionResult" /> indicating the result of the delete operation.</returns>
    [HttpDelete("{plannedTransactionId}")]
    [Transaction]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] Ulid plannedTransactionId,
        [FromQuery] DeletePlannedTransactionRequest request,
        CancellationToken cancellationToken)
    {
        await deletePlannedTransactionUseCase.ExecuteAsync(plannedTransactionId, request, cancellationToken);

        return NoContent();
    }
}