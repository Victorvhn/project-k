using Mediator;
using ProjectK.Core.Dtos.v1.Monthly;

namespace ProjectK.Core.Queries.v1.Summary;

public sealed record GetMonthlyOverviewByCategoryQuery(
    int Year,
    int Month
) : IQuery<IEnumerable<MonthlyExpensesOverviewData>>;