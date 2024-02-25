using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Api.Dtos.v1.Categories.Responses;

namespace ProjectK.Api.UseCases.v1.Categories.Interfaces;

public interface IUpdateCategoryUseCase
{
    Task<CategoryDto?> ExecuteAsync(Ulid categoryId, SaveCategoryRequest request,
        CancellationToken cancellationToken = default);
}