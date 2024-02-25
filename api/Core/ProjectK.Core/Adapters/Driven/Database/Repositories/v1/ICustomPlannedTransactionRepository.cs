using ProjectK.Core.Adapters.Driven.Database.Repositories.Base;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Adapters.Driven.Database.Repositories.v1;

public interface ICustomPlannedTransactionRepository : IRepositoryBase<CustomPlannedTransaction, Ulid>
{
    Task<bool> AnyByPlannedTransactionIdAsync(Ulid plannedTransactionId);
}