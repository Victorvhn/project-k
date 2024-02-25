using ProjectK.Api.Dtos.v1.Categories.Responses;

namespace ProjectK.Api.UseCases.v1.Categories.Interfaces;

public interface IGetCategoryUseCase
{
    Task<CategoryDto?> ExecuteAsync(Ulid categoryId,
        CancellationToken cancellationToken = default);
}