using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Queries.v1.PlannedTransactions;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions;

internal class GetPlannedTransactionPaginatedUseCase(
    IMapper mapper,
    ISender mediator) : IGetPlannedTransactionPaginatedUseCase
{
    public async Task<PaginatedResponse<PlannedTransactionDto>> ExecuteAsync(PaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPaginatedPlannedTransactionsQuery(request.CurrentPage, request.PageSize);

        var result = await mediator.Send(query, cancellationToken);

        return mapper.Map<PaginatedResponse<PlannedTransactionDto>>(result);
    }
}