using Domain.Common;
using Domain.Common.Models;
using Domain.Constants;

namespace Domain.Entities.Photos;

public abstract class Photo : Entity
{
    public abstract string Path { get; protected set; }
    public abstract bool IsMain { get; protected set; }
}