using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Api.Validators.v1.PlannedTransactions;

[ExcludeFromCodeCoverage]
public class UpdatePlannedTransactionRequestValidator : AbstractValidator<UpdatePlannedTransactionRequest>
{
    public UpdatePlannedTransactionRequestValidator()
    {
        RuleFor(x => x.ActionType)
            .IsInEnum();
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(PlannedTransaction.Constraints.DescriptionMaxLength);
        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(r => r.Year)
            .GreaterThan(0)
            .LessThan(3000);
        RuleFor(r => r.Month)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12);

        RuleFor(x => x.AmountType)
            .IsInEnum();
        RuleFor(x => x.Type)
            .IsInEnum();
        RuleFor(x => x.Recurrence)
            .IsInEnum();
        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .LessThanOrEqualTo(x => x.EndsAt)
            .When(w => w.EndsAt.HasValue);
        RuleFor(x => x.EndsAt)
            .GreaterThanOrEqualTo(x => x.StartsAt)
            .When(w => w.EndsAt.HasValue);
        RuleFor(r => r.CategoryId)
            .NotEqual(Ulid.Empty)
            .When(w => w.CategoryId.HasValue);
    }
}