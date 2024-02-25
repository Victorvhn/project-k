using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Api.Dtos.v1.Transactions.Responses;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Core.Queries.v1.Transactions;

namespace ProjectK.Api.UseCases.v1.Monthly;

internal class GetMonthlyTransactionsUseCase(
    IMapper mapper,
    ISender mediator) : IGetMonthlyTransactionsUseCase
{
    public async Task<IEnumerable<MonthlyTransactionDto>> ExecuteAsync(MonthlyRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMonthlyTransactionsQuery(request.Year, request.Month);

        var result = await mediator.Send(query, cancellationToken);

        return mapper.Map<IEnumerable<MonthlyTransactionDto>>(result);
    }
}