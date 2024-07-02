using Domain.Common;
using Domain.Common.Models;
using Domain.Constants;

namespace Domain.Entities.Photos;

public class Photo : Entity
{
    public string Paths { get; protected set; }
    public bool IsMain { get; protected set; }

    protected Photo(string paths, bool isMain)
    {
        Paths = paths;
        IsMain = isMain;
    }
    public static Result<object> CreateAndActivate(string path, string contentType, long length, bool isMain)
    {
        if (contentType != PhotoConstants.JPG && 
            contentType != PhotoConstants.JPEG && 
            contentType != PhotoConstants.PNG)
            return Errors.UserErrors.FileTypeInvalid(contentType);
        if (length > 100000)
            return Errors.UserErrors.FileLengthInvalid(length);
        return new Photo(path, isMain);
    }
}