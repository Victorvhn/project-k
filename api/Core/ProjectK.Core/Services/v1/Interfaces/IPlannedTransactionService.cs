namespace ProjectK.Core.Services.v1.Interfaces;

public interface IPlannedTransactionService
{
    Task<bool> ExistsByIdAsync(Ulid id, CancellationToken cancellationToken = default);
}