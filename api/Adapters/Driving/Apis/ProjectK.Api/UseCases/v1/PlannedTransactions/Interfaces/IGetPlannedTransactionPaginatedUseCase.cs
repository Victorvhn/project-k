using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;

public interface IGetPlannedTransactionPaginatedUseCase
{
    Task<PaginatedResponse<PlannedTransactionDto>> ExecuteAsync(PaginatedRequest request,
        CancellationToken cancellationToken = default);
}