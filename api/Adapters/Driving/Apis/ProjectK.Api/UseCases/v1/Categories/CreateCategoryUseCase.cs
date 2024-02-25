using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Core.Commands.v1.Categories;
using CategoryDto = ProjectK.Api.Dtos.v1.Categories.Responses.CategoryDto;

namespace ProjectK.Api.UseCases.v1.Categories;

public class CreateCategoryUseCase(
    IMapper mapper,
    ISender sender)
    : ICreateCategoryUseCase
{
    public async Task<CategoryDto?> ExecuteAsync(SaveCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateCategoryCommand(request.Name, request.HexColor);

        var result = await sender.Send(command, cancellationToken);

        return mapper.Map<CategoryDto?>(result);
    }
}