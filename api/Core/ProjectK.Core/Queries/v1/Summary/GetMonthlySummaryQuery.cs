using Mediator;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Enums;

namespace ProjectK.Core.Queries.v1.Summary;

public sealed record GetMonthlySummaryQuery(
    TransactionType TransactionType,
    int Year,
    int Month
) : IQuery<SummaryDto>;