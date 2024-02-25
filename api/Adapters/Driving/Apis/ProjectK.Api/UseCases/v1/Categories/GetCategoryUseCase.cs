using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Categories.Responses;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Core.Queries.v1.Categories;

namespace ProjectK.Api.UseCases.v1.Categories;

internal class GetCategoryUseCase(IMapper mapper, ISender mediator) : IGetCategoryUseCase
{
    public async Task<CategoryDto?> ExecuteAsync(Ulid categoryId, CancellationToken cancellationToken = default)
    {
        var query = new GetCategoryByIdQuery(categoryId);

        var category = await mediator.Send(query, cancellationToken);

        return mapper.Map<CategoryDto?>(category);
    }
}