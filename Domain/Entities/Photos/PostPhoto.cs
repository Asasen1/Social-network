using Domain.Common;
using Domain.Constants;

namespace Domain.Entities.Photos;

public class PostPhoto : Photo
{
    public sealed override string Path { get; protected set; }
    public sealed override bool IsMain { get; protected set; }

    protected PostPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public static Result<PostPhoto> CreateAndActivate(string path, string contentType, long length, bool isMain)
    {
        if (path.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(path));
        if (contentType != PhotoConstants.JPG && 
            contentType != PhotoConstants.JPEG && 
            contentType != PhotoConstants.PNG)
            return Errors.UserErrors.FileTypeInvalid(contentType);
        if (length > 100000)
            return Errors.UserErrors.FileLengthInvalid(length);
        return new PostPhoto(path, isMain);
    }

}