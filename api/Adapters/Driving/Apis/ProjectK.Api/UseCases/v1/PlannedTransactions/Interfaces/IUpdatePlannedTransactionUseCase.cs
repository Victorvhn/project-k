using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;

public interface IUpdatePlannedTransactionUseCase
{
    Task<PlannedTransactionDto?> ExecuteAsync(Ulid plannedTransactionId, UpdatePlannedTransactionRequest request,
        CancellationToken cancellationToken = default);
}