using Application.Abstractions;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commands.UserCreate;

public class CreateUserCommand : ICommandHandler<CreateUserRequest>
{
    private readonly WriteDbContext _writeDbContext;

    public CreateUserCommand(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<Result> Handle(CreateUserRequest request, CancellationToken ct)
    {
        var nickname =await _writeDbContext.Users
            .FirstOrDefaultAsync(u => u.Nickname == request.Nickname, cancellationToken: ct);
        if (nickname != null)
            return Errors.UserErrors.NotUnique(nameof(request.Nickname));
        
        var isParse = DateOnly.TryParse(request.BirthDate, out var birth);
        if (!isParse)
            return Errors.General.ValueIsInvalid(nameof(request.BirthDate));
        
        var user = User.Create(
            request.FirstName,
            request.SecondName,
            request.Email,
            request.Nickname,
            birth,
            request.Description);
        
        if (user.IsFailure)
            return user.Error;
        
        await _writeDbContext.Users.AddAsync(user.Value, ct);
        await _writeDbContext.SaveChangesAsync(ct);
        return Result.Success();
    }
}