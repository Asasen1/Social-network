using Application.Abstractions;
using Domain.Common;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commands.AddFriend;

public class AddFriendCommand(WriteDbContext dbContext) : ICommandHandler<AddFriendRequest>
{
    public async Task<Result> Handle(AddFriendRequest request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);
        var friend = await dbContext.Users
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Id == request.FriendId, ct);

        if (user == null)
            return Errors.General.NotFound(request.UserId);
        if (friend == null)
            return Errors.General.NotFound(request.FriendId);
        if (friend.Friends.Contains(user))
            return Errors.UserErrors.HasFriend(nameof(friend));
        
        var idResult = user.AddFriend(friend);

        if (idResult.IsFailure)
            return idResult.Error;

        await dbContext.SaveChangesAsync(ct);
        
        return Result.Success();
    }
}