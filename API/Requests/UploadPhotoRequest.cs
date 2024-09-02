namespace API.Requests;

public record UploadPhotoRequest(Guid UserId, IFormFile File, bool IsMain);