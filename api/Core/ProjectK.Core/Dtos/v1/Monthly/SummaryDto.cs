namespace ProjectK.Core.Dtos.v1.Monthly;

public record SummaryDto(
    decimal Expected,
    decimal Current
)
{
    public decimal Difference => Current - Expected;

    public decimal Percentage => CalculatePercentage();

    private decimal CalculatePercentage()
    {
        if (Expected == 0)
            return Current == 0
                ? 100
                : Current;

        if (Current == 0) return 0;

        return Current / Expected * 100;
    }
}