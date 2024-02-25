using ProjectK.Api.Dtos.v1.Transactions.Requests;
using ProjectK.Api.Dtos.v1.Transactions.Responses;

namespace ProjectK.Api.UseCases.v1.Transactions.Interfaces;

public interface ICreateTransactionUseCase
{
    Task<TransactionDto?> ExecuteAsync(SaveTransactionRequest request, CancellationToken cancellationToken = default);
}