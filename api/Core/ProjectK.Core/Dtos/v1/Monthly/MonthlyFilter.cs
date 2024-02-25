namespace ProjectK.Core.Dtos.v1.Monthly;

public record MonthlyFilter(
    int Year,
    int Month
)
{
    public DateOnly StartOfTheMonth => new(Year, Month, 1);
    public DateOnly EndOfTheMonth => new(Year, Month, DateTime.DaysInMonth(Year, Month));
}