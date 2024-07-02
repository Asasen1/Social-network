using Domain.Common;
using Domain.Constants;

namespace Domain.Entities.Photos;

public class UserPhoto : Photo
{
    private UserPhoto(string paths, bool isMain) : base(paths, isMain)
    {
    }
}