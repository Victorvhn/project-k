using System.Linq.Expressions;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Database.QueryExpressions;

internal static class MonthlyFilterExpressions
{
    public static class PlannedTransactions
    {
        public static Expression<Func<PlannedTransaction, bool>> IsFromDate(MonthlyFilter filter)
        {
            return plannedTransaction => plannedTransaction.StartsAt <= filter.EndOfTheMonth
                                         && (plannedTransaction.EndsAt == null ||
                                             plannedTransaction.EndsAt.Value >= filter.StartOfTheMonth)
                                         && (plannedTransaction.Recurrence == Recurrence.Monthly ||
                                             (plannedTransaction.Recurrence == Recurrence.Annual &&
                                              plannedTransaction.StartsAt.Month == filter.Month));
        }
    }
}