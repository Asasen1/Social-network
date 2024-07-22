using Domain.Constants;
using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.FullName).ChildRules(rules =>
        {
            rules.RuleFor(f => f.FirstName).NotEmpty().NotNull().MinimumLength(1).MaximumLength(20).WithMessage("");
            rules.RuleFor(f => f.SecondName).NotEmpty().NotNull().MinimumLength(1).MaximumLength(20);
        });
        RuleFor(u => u.Description).MaximumLength(50);
        RuleFor(u => u.Nickname).MaximumLength(10).NotEmpty().NotNull().MinimumLength(3);
    }
}