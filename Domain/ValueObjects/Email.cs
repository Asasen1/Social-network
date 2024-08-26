using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }

    public Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string input)
    {
        input = input.Trim();

        if (input.Length is < 1 or > Constraints.UserConstraints.MAX_LENGTH_NAME)
            return Errors.General.InvalidLength("email");

        if (Regex.IsMatch(input, "^(.+)@(.+)$") == false)
            return Errors.General.ValueIsInvalid("email");

        return new Email(input);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}