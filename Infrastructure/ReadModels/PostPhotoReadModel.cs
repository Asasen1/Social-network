namespace Infrastructure.ReadModels;

public class PostPhotoReadModel
{
    public Guid Id { get; init; } = Guid.Empty;
    public Guid PostId { get; init; }
    public string Path { get; init; } = string.Empty;
    public bool IsMain { get; init; }
}