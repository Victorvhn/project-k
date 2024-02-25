using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Core.Enums;
using ProjectK.Core.Queries.v1.Summary;
using SummaryDto = ProjectK.Api.Dtos.v1.Monthly.Responses.SummaryDto;

namespace ProjectK.Api.UseCases.v1.Monthly;

internal class GetMonthlySummaryUseCase(
    IMapper mapper,
    ISender mediator) : IGetMonthlySummaryUseCase
{
    public async Task<SummaryDto> ExecuteAsync(MonthlyRequest request, TransactionType transactionType,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMonthlySummaryQuery(transactionType, request.Year, request.Month);

        var result = await mediator.Send(query, cancellationToken);

        return mapper.Map<SummaryDto>(result);
    }
}