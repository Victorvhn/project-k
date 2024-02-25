using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;

public interface IGetPlannedTransactionForDateUseCase
{
    Task<PlannedTransactionDto?> ExecuteAsync(Ulid plannedTransactionId, MonthlyRequest monthlyRequest,
        CancellationToken cancellationToken = default);
}