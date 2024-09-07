using Application.Abstractions;
using Application.Providers;
using Domain.Common;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commands.DeletePhoto;

public class DeletePhotoCommand : ICommandHandler<DeletePhotoRequest>
{
    private readonly IFileProvider _provider;
    private readonly WriteDbContext _dbContext;

    public DeletePhotoCommand(IFileProvider provider, WriteDbContext dbContext)
    {
        _provider = provider;
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeletePhotoRequest request, CancellationToken ct)
    {
        var user = await _dbContext.Users.Include(user => user.Photos)
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct);
        if (user == null)
            return Errors.General.NotFound();
        
        var result = await _provider.RemovePhoto(request.Path, ct);
        if (result.IsFailure)
            return result.Error;
        
        var userPhoto = user.Photos.FirstOrDefault(p => p.Path == request.Path);
        if (userPhoto == null)
            return Errors.General.RemoveFailure(request.Path);
        
        user.RemovePhoto(userPhoto);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Success();
    }
}