using ProjectK.Core.Enums;

namespace ProjectK.Api.Dtos.v1.Monthly.Responses;

public record MonthlyPlannedTransactionDto(
    Ulid Id,
    string Description,
    decimal Amount,
    AmountType AmountType,
    TransactionType Type,
    Recurrence Recurrence,
    DateOnly ReferringDate,
    DateOnly StartsAt,
    DateOnly? EndsAt,
    Ulid? CategoryId,
    Ulid? PaidTransactionId,
    bool Paid,
    DateOnly? PaidAt,
    decimal? PaidAmount,
    string[] Tags,
    bool Ignore
);