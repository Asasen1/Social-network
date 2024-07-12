using Domain.Common;
using Domain.Common.Models;
using Domain.Constants;

namespace Domain.Entities.Photos;

public class PostPhoto : Entity
{
    public string Path { get; private set; }
    public bool IsMain { get; private set; }
   

    public PostPhoto()
    {
        
    }
    private PostPhoto(string path, bool isMain)
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