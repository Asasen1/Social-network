using Domain.Common;
using Domain.Common.Models;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : Entity
{
    public string PasswordHash { get; private set; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public string Nickname { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; }
    public IReadOnlyList<User> Friends => _friends;
    private readonly List<User> _friends = [];
    public IReadOnlyList<Post> Posts => _posts;
    private readonly List<Post> _posts = [];
    public IReadOnlyList<UserPhoto> Photos => _photos;
    private readonly List<UserPhoto> _photos = [];

    private User()
    {
    }

    private User(
        FullName fullName,
        Email email,
        string nickname,
        DateOnly? birthDate,
        string? description,
        DateTimeOffset createdDate)
    {
        FullName = fullName;
        Nickname = nickname;
        BirthDate = birthDate;
        Description = description;
        CreatedDate = createdDate;
    }
    
    public static Result<User> Create(
        string firstName,
        string secondName,
        string email,
        string nickname,
        DateOnly? birthDate,
        string? description)
    {
        firstName = firstName.Trim();
        secondName = secondName.Trim();
        
        if (firstName.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(firstName));
        if (secondName.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(secondName));
        
        var fullname = FullName.Create(firstName, secondName);
        if (fullname.IsFailure)
            return fullname.Error;

        var mail = ValueObjects.Email.Create(email);
        if (mail.IsFailure)
            return mail.Error;
        
        return new User(
            fullname.Value,
            mail.Value,
            nickname,
            birthDate,
            description,
            DateTimeOffset.UtcNow);
    }

    public Result<List<User>> AddFriend(User friend)
    {
        if (_friends.Contains(friend))
            return Errors.UserErrors.HasFriend(nameof(friend));
        if (friend._friends.Contains(this))
            return Errors.UserErrors.HasFriend();
        _friends.Add(friend);
        return _friends;
    }

    public Result<List<User>> DeleteFriend(User friend)
    {
        _friends.Remove(friend);
        return _friends;
    }
    public Result<List<UserPhoto>> AddPhoto(UserPhoto photo)
    {
        _photos.Add(photo);
        return _photos;
    }
    public Result<List<UserPhoto>> RemovePhoto(UserPhoto photo)
    {
        _photos.Remove(photo);
        return _photos;
    }
}