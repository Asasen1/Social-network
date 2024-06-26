using Domain.Common;
using Domain.Common.Models;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : Entity
{
    public FullName FullName { get; private set; }
    public DateTimeOffset BirthDate { get; private set; }
    private static readonly List<User> Friends = [];
    
    private User(Guid id, FullName fullName, DateTimeOffset birthDate) : base(id)
    {
        FullName = fullName;
        BirthDate = birthDate;
    }
    
    public static Result<User> Create(Guid id, string firstName, string secondName, DateTimeOffset birthDate)
    {
        return new User(
            id,
            FullName.Create(firstName, secondName).Value,
            birthDate);
    }

    public static Result<List<User>> AddFriend(User friend)
    {
        Friends.Add(friend);
        return Friends;
    }
}
