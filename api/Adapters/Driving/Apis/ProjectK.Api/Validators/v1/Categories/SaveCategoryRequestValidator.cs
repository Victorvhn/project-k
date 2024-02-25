using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Core.Entities;

namespace ProjectK.Api.Validators.v1.Categories;

[ExcludeFromCodeCoverage]
public class SaveCategoryRequestValidator : AbstractValidator<SaveCategoryRequest>
{
    public SaveCategoryRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MaximumLength(Category.Constraints.NameMaxLength);

        RuleFor(r => r.HexColor)
            .NotEmpty()
            .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$");
    }
}