using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.Categories.Responses;

namespace ProjectK.Api.UseCases.v1.Categories.Interfaces;

public interface IGetPaginatedCategoriesUseCase
{
    Task<PaginatedResponse<CategoryDto>> ExecuteAsync(PaginatedRequest request,
        CancellationToken cancellationToken = default);
}