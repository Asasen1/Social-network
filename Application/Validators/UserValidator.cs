using Application.CommonValidators;
using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.FullName).ChildRules(rules =>
        {
            rules.RuleFor(f => f.FirstName)
                // .NotEmptyWithError()
                // .NotNullWithError()
                .MinimumLengthWithError(1)
                .MaximumLengthWithError(20);
            rules.RuleFor(f => f.SecondName)
                .NotEmptyWithError()
                .NotNullWithError()
                .MinimumLengthWithError(1)
                .MaximumLengthWithError(20);
        });
        RuleFor(u => u.Description)!
            .MaximumLengthWithError(50);
        RuleFor(u => u.Nickname)
            .MaximumLengthWithError(10)
            .NotEmptyWithError()
            .NotNullWithError()
            .MinimumLengthWithError(3);
        RuleFor(u => u.Friends.Count.ToString()).MaximumLengthWithError(10000);
        RuleFor(u => u.Posts.Count.ToString()).MaximumLengthWithError(1000);
        RuleFor(u => u.Photos.Count.ToString()).MaximumLengthWithError(1000);
    }
}