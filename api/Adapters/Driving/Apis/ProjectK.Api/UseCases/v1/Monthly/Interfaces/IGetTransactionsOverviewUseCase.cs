using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;

namespace ProjectK.Api.UseCases.v1.Monthly.Interfaces;

public interface IGetTransactionsOverviewUseCase
{
    Task<IEnumerable<MonthlyExpensesOverviewResponse>> ExecuteAsync(MonthlyRequest request,
        CancellationToken cancellationToken = default);
}