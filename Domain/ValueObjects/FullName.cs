﻿using Domain.Common;
using Domain.Common.Models;
using Domain.Constants;

namespace Domain.ValueObjects;

public class FullName : ValueObject
{
    public string FirstName { get; }
    public string SecondName { get; }

   
    private FullName(string firstName, string secondName)
    {
        FirstName = firstName;
        SecondName = secondName;
    }

    public static Result<FullName> Create(string firstName, string secondName)
    {
        firstName = firstName.Trim().ToLower();
        secondName = secondName.Trim().ToLower();
        firstName = char.ToUpper(firstName[0]) + firstName[1..];
        secondName = char.ToUpper(secondName[0]) + secondName[1..];
        if (firstName.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(firstName));
        if (secondName.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(secondName));
        if (firstName.Length < UserConstants.MINIMUM_LENGTH_NAME ||
            firstName.Length > UserConstants.MAXIMUM_LENGTH_NAME)
            return Errors.General.InvalidLength(nameof(firstName));
        if (secondName.Length < UserConstants.MINIMUM_LENGTH_NAME ||
            secondName.Length > UserConstants.MAXIMUM_LENGTH_NAME)
            return Errors.General.InvalidLength(nameof(firstName));
        return new FullName(firstName, secondName);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return FirstName;
        yield return SecondName;
    }
    
}