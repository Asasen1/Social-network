using Domain.Common;
using Domain.Entities;
using Infrastructure.DbContexts;

namespace Infrastructure.Commands.UserCreate;

public class CreateUserCommand
{
    private readonly WriteDbContext _writeDbContext;

    public CreateUserCommand(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<Result<User>> Handle(CreateUserRequest request, CancellationToken ct)
    {
        var user = User.Create(
            request.FirstName,
            request.SecondName,
            request.Nickname,
            null,
            request.Description).Value;
        await _writeDbContext.Users.AddAsync(user, ct);
        await _writeDbContext.SaveChangesAsync(ct);
        return user;
    }
}