using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;

public interface IPayPlannedTransactionUseCase
{
    Task ExecuteAsync(Ulid plannedTransactionId, MonthlyRequest monthlyRequest, PayPlannedTransactionRequest request, CancellationToken cancellationToken = default);
}