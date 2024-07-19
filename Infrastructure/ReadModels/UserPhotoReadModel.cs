namespace Infrastructure.ReadModels;

public class UserPhotoReadModel
{
    public Guid Id { get; init; } = Guid.Empty;
    public Guid UserId { get; init; }
    public string Path { get; init; } = string.Empty;
    public bool IsMain { get; init; }
    // public List<LikeReadModel> Likes = [];
}