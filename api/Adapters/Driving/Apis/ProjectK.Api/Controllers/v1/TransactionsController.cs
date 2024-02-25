using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProjectK.Api.Attributes;
using ProjectK.Api.Dtos.v1.Transactions.Requests;
using ProjectK.Api.Dtos.v1.Transactions.Responses;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;

namespace ProjectK.Api.Controllers.v1;

[ApiVersion("1")]
public class TransactionsController(
    ICreateTransactionUseCase createTransactionUseCase,
    IUpdateTransactionUseCase updateTransactionUseCase,
    IDeleteTransactionUseCase deleteTransactionUseCase,
    IGetTransactionUseCase getTransactionUseCase) : ApiControllerBase
{
    /// <summary>
    ///     Creates a transaction.
    /// </summary>
    /// <param name="request">The transaction creation request.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns the created transaction.</returns>
    [HttpPost]
    [Transaction]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TransactionDto>> Create([FromBody] SaveTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await createTransactionUseCase.ExecuteAsync(request, cancellationToken);

        return Created("/", result);
    }

    /// <summary>
    ///     Updates a transaction.
    /// </summary>
    /// <param name="transactionId">The ID of the transaction to be updated.</param>
    /// <param name="request">The request body containing updated details.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns the updated transaction.</returns>
    [HttpPut("{transactionId}")]
    [Transaction]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransactionDto>> Update([FromRoute] Ulid transactionId,
        [FromBody] SaveTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await updateTransactionUseCase.ExecuteAsync(transactionId, request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Deletes a transaction.
    /// </summary>
    /// <param name="transactionId">The ID of the transaction to be deleted.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns an <see cref="ActionResult" /> indicating the result of the delete operation.</returns>
    [HttpDelete("{transactionId}")]
    [Transaction]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] Ulid transactionId,
        CancellationToken cancellationToken)
    {
        await deleteTransactionUseCase.ExecuteAsync(transactionId, cancellationToken);

        return NoContent();
    }


    /// <summary>
    ///     Retrieves a transaction by ID.
    /// </summary>
    /// <param name="transactionId">The ID of the transaction to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the transaction.</returns>
    [HttpGet("{transactionId}")]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransactionDto>> GetById([FromRoute] Ulid transactionId,
        CancellationToken cancellationToken)
    {
        var result =
            await getTransactionUseCase.ExecuteAsync(transactionId, cancellationToken);

        return Ok(result);
    }
}