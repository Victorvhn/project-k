namespace ProjectK.Api.Dtos.v1.Monthly.Responses;

public record MonthlyExpensesOverviewResponse(
    string CategoryName,
    string CategoryHexColor,
    decimal PlannedAmount,
    decimal CurrentAmount
);