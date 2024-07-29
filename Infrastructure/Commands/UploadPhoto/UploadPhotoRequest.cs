using Microsoft.AspNetCore.Http;

namespace Infrastructure.Commands.UploadPhoto;

public record UploadPhotoRequest(Guid UserId, IFormFile File, bool IsMain);