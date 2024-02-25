using ProjectK.Core.Dtos.v1.Monthly;

namespace ProjectK.Core;

public static class DayOnlySafe
{
    public static int Get(MonthlyFilter monthlyFilter, int dayToCheck)
    {
        var daysInMonth = DateTime.DaysInMonth(monthlyFilter.Year, monthlyFilter.Month);
        
        var dayExistsInMonth = dayToCheck <= daysInMonth;

        return !dayExistsInMonth ? daysInMonth : dayToCheck;
    }
}