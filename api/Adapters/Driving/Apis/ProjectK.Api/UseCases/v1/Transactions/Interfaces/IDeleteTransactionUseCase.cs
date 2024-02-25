namespace ProjectK.Api.UseCases.v1.Transactions.Interfaces;

public interface IDeleteTransactionUseCase
{
    Task ExecuteAsync(Ulid transactionId, CancellationToken cancellationToken = default);
}