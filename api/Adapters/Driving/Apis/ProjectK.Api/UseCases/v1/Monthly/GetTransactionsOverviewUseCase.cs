using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Core.Queries.v1.Summary;

namespace ProjectK.Api.UseCases.v1.Monthly;

internal class GetTransactionsOverviewUseCase(
    IMapper mapper,
    ISender mediator) : IGetTransactionsOverviewUseCase
{
    public async Task<IEnumerable<MonthlyExpensesOverviewResponse>> ExecuteAsync(MonthlyRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMonthlyOverviewByCategoryQuery(request.Year, request.Month);

        var result = await mediator.Send(query, cancellationToken);

        return mapper.Map<IEnumerable<MonthlyExpensesOverviewResponse>>(result);
    }
}