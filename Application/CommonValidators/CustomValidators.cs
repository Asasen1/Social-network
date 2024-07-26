using Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.CommonValidators;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());
        });
    }

    public static IRuleBuilderOptions<T, TProperty> NotEmptyWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }

    public static IRuleBuilderOptions<T, TProperty> NotNullWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .WithError(Errors.General.ValueIsRequired());
    }


    public static IRuleBuilderOptions<T, string> MaximumLengthWithError<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int maxLength)
    {
        return ruleBuilder
            .MaximumLength(maxLength)
            .WithError(Errors.General.InvalidLength());
    }

    public static IRuleBuilderOptions<T, string> MinimumLengthWithError<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int minLength)
    {
        return ruleBuilder
            .MinimumLength(minLength)
            .WithError(Errors.General.InvalidLength());
    }

    public static IRuleBuilderOptions<T, TProperty> GreaterThanWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, TProperty valueToCompare)
        where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .GreaterThan(valueToCompare)
            .WithError(Errors.General.InvalidLength());
    }

    public static IRuleBuilderOptions<T, TProperty?> GreaterThanWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder, TProperty valueToCompare)
        where TProperty : struct, IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .GreaterThan(valueToCompare)
            .WithError(Errors.General.InvalidLength());
    }

    public static IRuleBuilderOptions<T, TProperty> LessThanWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, TProperty valueToCompare)
        where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .LessThan(valueToCompare)
            .WithError(Errors.General.InvalidLength());
    }

    public static IRuleBuilderOptions<T, TProperty> MustBePhoto<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
        where TProperty : IFormFile
    {
        string[] allowedContentTypes = ["image/jpeg", "image/png", "image/png"];
        return ruleBuilder
            .Must((value) => allowedContentTypes.Contains(value.ContentType) && value.Length < 20000)
            .WithError(Errors.UserErrors.FileTypeInvalid(null));
        // return ruleBuilder.Custom((value, context) =>
        // {
        //     string[] allowedContentTypes = ["image/jpeg", "image/png", "image/png"];
        //     var contains = allowedContentTypes.Contains(value.ContentType);
        //     context.AddFailure(Errors.Volunteers.FileTypeInvalid(value.ContentType));
        // });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule
            .WithMessage(error.Serialize());
    }
}