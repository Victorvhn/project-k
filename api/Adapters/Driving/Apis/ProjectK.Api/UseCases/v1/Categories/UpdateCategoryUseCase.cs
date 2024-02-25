using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Api.Dtos.v1.Categories.Responses;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Core.Commands.v1.Categories;

namespace ProjectK.Api.UseCases.v1.Categories;

public class UpdateCategoryUseCase(
    IMapper mapper,
    ISender mediator)
    : IUpdateCategoryUseCase
{
    public async Task<CategoryDto?> ExecuteAsync(Ulid categoryId, SaveCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateCategoryCommand(categoryId, request.Name, request.HexColor);

        var category = await mediator.Send(command, cancellationToken);

        return mapper.Map<CategoryDto?>(category);
    }
}