using Application.Abstractions;
using Application.Providers;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DbContexts;

namespace Infrastructure.Commands.UploadPhoto;

public class UploadPhotoCommand : ICommandHandler<UploadPhotoData>
{
    private readonly IMinioProvider _provider;
    private readonly WriteDbContext _dbcontext;

    public UploadPhotoCommand(IMinioProvider provider, WriteDbContext dbcontext)
    {
        _provider = provider;
        _dbcontext = dbcontext;
    }

    public async Task<Result> Handle(UploadPhotoData data, CancellationToken ct)
    {
        var photoId = Guid.NewGuid();
        var path = photoId + Path.GetExtension(data.FileName);
        
        
        var photoResult = await _provider.UploadPhoto(data.Stream, path, ct);
        if (photoResult.IsFailure)
            return photoResult.Error;
        
        var user = await _dbcontext.Users.FindAsync(new object?[] { data.UserId, ct }, cancellationToken: ct);
        if (user == null)
            return Errors.General.NotFound(data.UserId);
        
        var userPhoto =
            UserPhoto.CreateAndActivate(path, data.ContentType, data.FileLength, data.IsMain);
        if (userPhoto.IsFailure)
            return userPhoto.Error;
        
        var publishPhoto = user.AddPhoto(userPhoto.Value);
        if (publishPhoto.IsFailure)
            return publishPhoto.Error;
        
        await _dbcontext.SaveChangesAsync(ct);
        return Result.Success();
    }
}