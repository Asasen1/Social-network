using Domain.Common;
using Domain.Common.Models;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Agregates;

public class User : Entity
{
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }
    public RefreshToken RefreshToken { get; private set; }
    
    public FullName FullName { get; private set; }
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
        Email email,
        string passwordHash,
        RefreshToken token,
        FullName fullName,
        string nickname,
        DateOnly? birthDate,
        string? description,
        DateTimeOffset createdDate)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = Role.User;
        RefreshToken = token;
        FullName = fullName;
        Nickname = nickname;
        BirthDate = birthDate;
        Description = description;
        CreatedDate = createdDate;
    }
    
    public static Result<User> Create(
        Email email,
        string passwordHash,
        FullName fullName,
        string nickname,
        DateOnly? birthDate,
        string? description)
    {
        var token = RefreshToken.Create(string.Empty, DateTime.MinValue).Value;
        
        return new User(
            email,
            passwordHash,
            token,
            fullName,
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

    public Result<RefreshToken> UpdateRefresh(RefreshToken token)
    {
        RefreshToken = token;
        return token;
    }
}