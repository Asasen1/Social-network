namespace Application.DTO;

public class PhotoDto
{
    public PhotoDto(Guid id, string path, bool isMain,
        Guid userId)
    {
        Id = id;
        Path = path;
        IsMain = isMain;
        UserId = userId;
    }

    public Guid Id { get; init; }
    public string Path { get; init; } 
    public bool IsMain { get; init; }
    public Guid UserId { get; init; }

    public PhotoDto()
    {
        
    }
}