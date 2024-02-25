using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Api.Dtos.v1.Categories.Responses;

namespace ProjectK.Api.UseCases.v1.Categories.Interfaces;

public interface ICreateCategoryUseCase
{
    Task<CategoryDto?> ExecuteAsync(SaveCategoryRequest request,
        CancellationToken cancellationToken = default);
}