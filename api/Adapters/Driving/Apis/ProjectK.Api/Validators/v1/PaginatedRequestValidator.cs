using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using ProjectK.Api.Dtos.v1;

namespace ProjectK.Api.Validators.v1;

[ExcludeFromCodeCoverage]
public class PaginatedRequestValidator : AbstractValidator<PaginatedRequest>
{
    public PaginatedRequestValidator()
    {
        RuleFor(r => r.PageSize)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.CurrentPage)
            .GreaterThanOrEqualTo(1);
    }
}