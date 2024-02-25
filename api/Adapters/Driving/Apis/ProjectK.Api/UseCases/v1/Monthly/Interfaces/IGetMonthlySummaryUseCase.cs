using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Core.Enums;

namespace ProjectK.Api.UseCases.v1.Monthly.Interfaces;

public interface IGetMonthlySummaryUseCase
{
    Task<SummaryDto> ExecuteAsync(MonthlyRequest request, TransactionType transactionType,
        CancellationToken cancellationToken = default);
}