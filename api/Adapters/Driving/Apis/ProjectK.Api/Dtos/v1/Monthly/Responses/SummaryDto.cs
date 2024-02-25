namespace ProjectK.Api.Dtos.v1.Monthly.Responses;

public record SummaryDto(
    decimal Expected,
    decimal Current,
    decimal Difference,
    decimal Percentage
);