namespace Application.DTO;

public class PhotoDto
{
    public PhotoDto(Guid id, string path, bool isMain)
    {
        Id = id;
        Path = path;
        IsMain = isMain;
    }

    public Guid Id { get; init; }
    public string Path { get; init; } 
    public bool IsMain { get; init; }
    

    public PhotoDto()
    {
        
    }
}