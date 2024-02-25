using ProjectK.Api.Dtos.v1.Transactions.Responses;

namespace ProjectK.Api.UseCases.v1.Transactions.Interfaces;

public interface IGetTransactionUseCase
{
    Task<TransactionDto?> ExecuteAsync(Ulid transactionId, CancellationToken cancellationToken = default);
}