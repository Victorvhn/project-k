using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using ProjectK.Api.Dtos.v1.Monthly.Requests;

namespace ProjectK.Api.Validators.v1.Monthly;

[ExcludeFromCodeCoverage]
public class MonthlyRequestValidator : AbstractValidator<MonthlyRequest>
{
    public MonthlyRequestValidator()
    {
        RuleFor(r => r.Year)
            .GreaterThan(0)
            .LessThan(3000);
        RuleFor(r => r.Month)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12);
    }
}