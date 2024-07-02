using Domain.Common;
using Domain.Constants;

namespace Domain.Entities.Photos;

public class PostPhoto : Photo
{
    public PostPhoto(string paths, bool isMain) : base(paths, isMain)
    {
    }
    public new static Result<object> CreateAndActivate(string path, string contentType, long length, bool isMain)
    {
        if (contentType != PhotoConstants.JPG && 
            contentType != PhotoConstants.JPEG && 
            contentType != PhotoConstants.PNG)
            return Errors.UserErrors.FileTypeInvalid(contentType);
        if (length > 100000)
            return Errors.UserErrors.FileLengthInvalid(length);
        return new PostPhoto(path, isMain);
    }
}