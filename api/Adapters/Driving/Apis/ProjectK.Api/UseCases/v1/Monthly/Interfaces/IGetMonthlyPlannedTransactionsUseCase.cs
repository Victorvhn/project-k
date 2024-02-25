using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;

namespace ProjectK.Api.UseCases.v1.Monthly.Interfaces;

public interface IGetMonthlyPlannedTransactionsUseCase
{
    Task<IEnumerable<MonthlyPlannedTransactionDto>> ExecuteAsync(MonthlyRequest request,
        CancellationToken cancellationToken = default);
}