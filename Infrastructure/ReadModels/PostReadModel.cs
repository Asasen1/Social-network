namespace Infrastructure.ReadModels;

public class PostReadModel
{
    public Guid Id { get; init; } = Guid.Empty;
    public Guid UserId { get; init; }
    public string Header { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public List<PostPhotoReadModel> Photos = [];
    public List<LikeReadModel> Likes = [];
    public List<CommentReadModel> Comments = [];
}