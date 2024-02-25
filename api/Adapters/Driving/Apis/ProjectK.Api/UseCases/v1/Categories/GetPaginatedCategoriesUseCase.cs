using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.Categories.Responses;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Core.Queries.v1.Categories;

namespace ProjectK.Api.UseCases.v1.Categories;

internal class GetPaginatedCategoriesUseCase(
    IMapper mapper,
    ISender mediator) : IGetPaginatedCategoriesUseCase
{
    public async Task<PaginatedResponse<CategoryDto>> ExecuteAsync(PaginatedRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPaginatedCategoriesQuery(request.CurrentPage, request.PageSize);

        var result = await mediator.Send(query, cancellationToken);

        return mapper.Map<PaginatedResponse<CategoryDto>>(result);
    }
}