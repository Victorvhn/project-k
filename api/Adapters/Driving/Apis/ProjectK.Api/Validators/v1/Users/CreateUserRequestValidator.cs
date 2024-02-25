using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using ProjectK.Api.Dtos.v1.Users.Requests;
using ProjectK.Core.Entities;

namespace ProjectK.Api.Validators.v1.Users;

[ExcludeFromCodeCoverage]
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MaximumLength(User.Constraints.NameMaxLength);

        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(User.Constraints.EmailMaxLength);
    }
}