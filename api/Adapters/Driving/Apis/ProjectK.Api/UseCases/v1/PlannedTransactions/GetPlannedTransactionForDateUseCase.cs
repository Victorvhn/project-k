using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Queries.v1.PlannedTransactions;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions;

internal class GetPlannedTransactionForDateUseCase(
    IMapper mapper,
    ISender mediator)
    : IGetPlannedTransactionForDateUseCase
{
    public async Task<PlannedTransactionDto?> ExecuteAsync(Ulid plannedTransactionId, MonthlyRequest monthlyRequest,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPlannedTransactionByIdAndDateQuery(plannedTransactionId, monthlyRequest.Year,
            monthlyRequest.Month);

        var plannedTransaction = await mediator.Send(query, cancellationToken);

        return mapper.Map<PlannedTransactionDto?>(plannedTransaction);
    }
}