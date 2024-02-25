using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Core.Queries.v1.PlannedTransactions;

namespace ProjectK.Api.UseCases.v1.Monthly;

internal class GetMonthlyPlannedTransactionsUseCase(
    IMapper mapper,
    ISender mediator) : IGetMonthlyPlannedTransactionsUseCase
{
    public async Task<IEnumerable<MonthlyPlannedTransactionDto>> ExecuteAsync(MonthlyRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMonthlyPlannedTransactionsQuery(request.Year, request.Month);

        var result = await mediator.Send(query, cancellationToken);

        return mapper.Map<IEnumerable<MonthlyPlannedTransactionDto>>(result);
    }
}