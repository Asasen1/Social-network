using Domain.Common;
using Domain.Common.Models;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : Entity
{
    public FullName FullName { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; }
    public IReadOnlyList<User> Friends => _friends;
    private readonly List<User> _friends = [];
    public IReadOnlyList<Post> Posts => _posts;
    private readonly List<Post> _posts = [];
    public IReadOnlyList<Photo> Photos => _photos;
    private readonly List<Photo> _photos = [];
    
    private User(
        Guid id, 
        FullName fullName, 
        DateOnly birthDate, 
        string description, 
        DateTimeOffset createdDate)
        : base(id)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Description = description;
        CreatedDate = createdDate;
    }

    public static Result<User> Create(
        Guid id, 
        string firstName,
        string secondName,
        DateOnly birthDate, 
        string description, 
        DateTimeOffset createdDate)
    {
        return new User(
            id,
            FullName.Create(firstName, secondName).Value,
            birthDate,
            description,
            DateTimeOffset.UtcNow);
    }

    public Result<List<User>> AddFriend(User friend)
    {
        _friends.Add(friend);
        return _friends;
    }
}
