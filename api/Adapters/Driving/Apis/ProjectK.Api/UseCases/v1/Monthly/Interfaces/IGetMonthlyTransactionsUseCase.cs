using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Api.Dtos.v1.Transactions.Responses;

namespace ProjectK.Api.UseCases.v1.Monthly.Interfaces;

public interface IGetMonthlyTransactionsUseCase
{
    Task<IEnumerable<MonthlyTransactionDto>> ExecuteAsync(MonthlyRequest request,
        CancellationToken cancellationToken = default);
}