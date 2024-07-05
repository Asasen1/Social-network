using Domain.Common;
using Domain.Entities;
using Infrastructure.DbContexts;

namespace Infrastructure.Commands.UserCreate;

public class CreateUserCommand
{
    private readonly SocialWriteDbContext _socialWriteDbContext;

    public CreateUserCommand(SocialWriteDbContext socialWriteDbContext)
    {
        _socialWriteDbContext = socialWriteDbContext;
    }

    public async Task<Result<User>> Handle(CreateUserRequest request, CancellationToken ct)
    {
        var user = User.Create(
            request.FirstName,
            request.SecondName,
            request.Nickname,
            null,
            request.Description).Value;
        await _socialWriteDbContext.Users.AddAsync(user, ct);
        await _socialWriteDbContext.SaveChangesAsync(ct);
        return user;
    }
}