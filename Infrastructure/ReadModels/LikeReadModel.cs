namespace Infrastructure.ReadModels;

public class LikeReadModel
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid PostId { get; private set; }
    public DateTime LikeDate { get; private set; }
}