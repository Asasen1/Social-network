using Domain.Common;

namespace Domain.ValueObjects;

public class FullName
{
    private string FirstName { get; set; }
    private string SecondName { get; set; }

    private FullName(string firstName, string secondName)
    {
        FirstName = firstName;
        SecondName = secondName;
    }

    public static Result<FullName> Create(string firstName, string secondName)
    {
        firstName = firstName.Trim();
        secondName = secondName.Trim();
        if (firstName.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(firstName));
        if (secondName.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(secondName));
        return new FullName(firstName, secondName);
    }

    // protected override IEnumerable<IComparable> GetEqualityComponents()
    // {
    //     yield return FirstName;
    //     yield return SecondName;
    // }
    
}