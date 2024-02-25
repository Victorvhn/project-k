using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;

public interface IDeletePlannedTransactionUseCase
{
    Task ExecuteAsync(Ulid plannedTransactionId,
        DeletePlannedTransactionRequest request,
        CancellationToken cancellationToken = default);
}