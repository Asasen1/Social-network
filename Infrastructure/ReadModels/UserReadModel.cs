using Domain.Entities;

namespace Infrastructure.ReadModels;

public class UserReadModel
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string SecondName { get; init; } = string.Empty;
    public string Nickname { get; init; } = string.Empty;
    public DateOnly? BirthDate { get; init; }
    public string? Description { get; init; } = string.Empty;
    public DateTimeOffset CreatedDate { get; init; }
    public List<UserReadModel> Friends = [];
    public List<PostReadModel> Posts = [];
    public List<UserPhotoReadModel> Photos = [];
}   