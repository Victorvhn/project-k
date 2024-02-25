namespace ProjectK.Core.Dtos.v1.Monthly;

public record MonthlyExpensesOverviewData(
    string CategoryName,
    string CategoryHexColor,
    decimal PlannedAmount,
    decimal CurrentAmount
);