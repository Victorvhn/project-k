using ProjectK.Api.Dtos.v1.Transactions.Requests;
using ProjectK.Api.Dtos.v1.Transactions.Responses;

namespace ProjectK.Api.UseCases.v1.Transactions.Interfaces;

public interface IUpdateTransactionUseCase
{
    Task<TransactionDto?> ExecuteAsync(Ulid transactionId, SaveTransactionRequest request,
        CancellationToken cancellationToken = default);
}