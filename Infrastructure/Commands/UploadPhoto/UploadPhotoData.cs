namespace Infrastructure.Commands.UploadPhoto;

public record UploadPhotoData(
    Guid UserId, 
    Stream Stream, 
    string FileName, 
    string ContentType, 
    long FileLength, 
    bool IsMain);