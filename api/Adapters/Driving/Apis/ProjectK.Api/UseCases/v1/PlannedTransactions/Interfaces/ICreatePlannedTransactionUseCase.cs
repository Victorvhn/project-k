using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;

public interface ICreatePlannedTransactionUseCase
{
    Task<PlannedTransactionDto?> ExecuteAsync(CreatePlannedTransactionRequest request,
        CancellationToken cancellationToken = default);
}