namespace ProjectK.Api.UseCases.v1.Categories.Interfaces;

public interface IDeleteCategoryUseCase
{
    Task ExecuteAsync(Ulid categoryId, CancellationToken cancellationToken = default);
}