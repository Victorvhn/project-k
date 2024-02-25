using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Core.Enums;

namespace ProjectK.Api.Validators.v1.PlannedTransactions;

[ExcludeFromCodeCoverage]
public class DeletePlannedTransactionRequestValidator : AbstractValidator<DeletePlannedTransactionRequest>
{
    public DeletePlannedTransactionRequestValidator()
    {
        RuleFor(x => x.ActionType)
            .IsInEnum();

        When(w => w.ActionType is ActionType.JustOne or ActionType.FromNowOn, () =>
        {
            RuleFor(r => r.Year)
                .GreaterThan(0)
                .LessThan(3000);
            RuleFor(r => r.Month)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(12);
        });
    }
}