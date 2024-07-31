using Application.Abstractions;
using Application.Providers;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DbContexts;

namespace Infrastructure.Commands.UploadPhoto;

public class UploadPhotoCommand : ICommandHandler<UploadPhotoRequest>
{
    private readonly IMinioProvider _provider;
    private readonly WriteDbContext _dbcontext;

    public UploadPhotoCommand(IMinioProvider provider, WriteDbContext dbcontext)
    {
        _provider = provider;
        _dbcontext = dbcontext;
    }

    public async Task<Result> Handle(UploadPhotoRequest request, CancellationToken ct)
    {
        var photoId = Guid.NewGuid();
        var path = photoId + Path.GetExtension(request.File.FileName);
        
        await using var stream = request.File.OpenReadStream();
        var photoResult = await _provider.UploadPhoto(stream, path, ct);
        if (photoResult.IsFailure)
            return photoResult.Error;
        
        var user = await _dbcontext.Users.FindAsync(new object?[] { request.UserId, ct }, cancellationToken: ct);
        if (user == null)
            return Errors.General.NotFound(request.UserId);
        
        var userPhoto =
            UserPhoto.CreateAndActivate(path, request.File.ContentType, request.File.Length, request.IsMain);
        if (userPhoto.IsFailure)
            return userPhoto.Error;
        
        var publishPhoto = user.AddPhoto(userPhoto.Value);
        if (publishPhoto.IsFailure)
            return publishPhoto.Error;
        
        await _dbcontext.SaveChangesAsync(ct);
        return Result.Success();
    }
}