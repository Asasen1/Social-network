namespace Infrastructure.ReadModels;

public class CommentReadModel
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid PostId { get; private set; }
    public string Text { get; private set; }
    public DateTime CommentDate { get; private set; }
}