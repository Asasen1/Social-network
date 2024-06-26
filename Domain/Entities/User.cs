﻿using Domain.Common;
using Domain.Common.Models;
using Domain.Entities.Photos;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : Entity
{
    public FullName FullName { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; }
    public IReadOnlyList<User> Friends => _friends;
    private readonly List<User> _friends = [];
    public IReadOnlyList<Post> Posts => _posts;
    private readonly List<Post> _posts = [];
    public IReadOnlyList<UserPhoto> Photos => _photos;
    private readonly List<UserPhoto> _photos = [];
    
    private User(
        FullName fullName, 
        DateOnly? birthDate, 
        string? description, 
        DateTimeOffset createdDate)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Description = description;
        CreatedDate = createdDate;
    }

    public static Result<User> Create(
        string firstName,
        string secondName,
        DateOnly? birthDate, 
        string? description, 
        DateTimeOffset createdDate)
    {
        firstName = firstName.Trim();
        secondName = secondName.Trim();
        if (firstName.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(firstName));
        if (secondName.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(secondName));
        
        return new User(
            FullName.Create(firstName, secondName).Value,
            birthDate,
            description,
            DateTimeOffset.UtcNow);
    }

    public Result<List<User>> AddFriend(User friend)
    {
        if (_friends.Contains(friend))
            return Errors.User.HasFriend(nameof(friend));
        _friends.Add(friend);
        return _friends;
    }
}
