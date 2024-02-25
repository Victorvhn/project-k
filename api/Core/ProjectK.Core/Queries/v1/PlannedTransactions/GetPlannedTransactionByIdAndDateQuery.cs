using Mediator;
using ProjectK.Core.Dtos.v1.PlannedTransactions;

namespace ProjectK.Core.Queries.v1.PlannedTransactions;

public sealed record GetPlannedTransactionByIdAndDateQuery(
    Ulid PlannedTransactionId,
    int Year,
    int Month
) : IQuery<PlannedTransactionDto?>;