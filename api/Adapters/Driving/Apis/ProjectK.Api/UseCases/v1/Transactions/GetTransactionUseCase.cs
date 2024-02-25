using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Transactions.Responses;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;
using ProjectK.Core.Queries.v1.Transactions;

namespace ProjectK.Api.UseCases.v1.Transactions;

internal class GetTransactionUseCase(IMapper mapper, ISender mediator) : IGetTransactionUseCase
{
    public async Task<TransactionDto?> ExecuteAsync(Ulid transactionId, CancellationToken cancellationToken = default)
    {
        var query = new GetTransactionByIdQuery(transactionId);

        var result = await mediator.Send(query, cancellationToken);

        return mapper.Map<TransactionDto?>(result);
    }
}