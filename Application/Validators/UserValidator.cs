using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Description).MaximumLength(50);
    }
}