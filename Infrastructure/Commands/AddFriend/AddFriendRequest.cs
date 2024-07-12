namespace Infrastructure.Commands.AddFriend;

public record AddFriendRequest(Guid FriendId, Guid UserId);