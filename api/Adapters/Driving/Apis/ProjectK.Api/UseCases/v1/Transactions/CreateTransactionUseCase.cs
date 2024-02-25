using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Transactions.Requests;
using ProjectK.Api.Dtos.v1.Transactions.Responses;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;
using ProjectK.Core.Commands.v1.Transactions;

namespace ProjectK.Api.UseCases.v1.Transactions;

internal class CreateTransactionUseCase(IMapper mapper, ISender mediator)
    : ICreateTransactionUseCase
{
    public async Task<TransactionDto?> ExecuteAsync(SaveTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateTransactionCommand(
            request.Description,
            request.Amount,
            request.Type,
            request.PaidAt,
            request.CategoryId,
            request.PlannedTransactionId);

        var result = await mediator.Send(command, cancellationToken);

        return mapper.Map<TransactionDto?>(result);
    }
}