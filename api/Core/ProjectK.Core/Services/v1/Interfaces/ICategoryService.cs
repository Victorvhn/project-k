namespace ProjectK.Core.Services.v1.Interfaces;

public interface ICategoryService
{
    Task<bool> ExistsByIdAsync(Ulid categoryId, CancellationToken cancellationToken = default);
}