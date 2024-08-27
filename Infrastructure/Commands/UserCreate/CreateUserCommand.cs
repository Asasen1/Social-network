using Application.Abstractions;
using Domain.Agregates;
using Domain.Common;
using Domain.ValueObjects;
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
        var validateUser = await _writeDbContext.Users
            .Where(u => u.Email.Value == request.Email
            && u.Nickname == request.Nickname)
            .ToListAsync(cancellationToken: ct);
            
        if (validateUser.Count != 0)
            return Errors.UserErrors.NotUnique("email or nickname");

        var birthDay = new DateOnly();
        if (request.BirthDate is not null)
        {
            var isParse = DateOnly.TryParse(request.BirthDate, out var birth);
            if (!isParse)
                return Errors.General.ValueIsInvalid(nameof(request.BirthDate));
            birthDay = birth;
        }

        var fullname = FullName.Create(request.FirstName, request.SecondName);
        if (fullname.IsFailure)
            return fullname.Error;

        var mail = Email.Create(request.Email);
        if (mail.IsFailure)
            return mail.Error;
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        if (passwordHash is null)
            return Errors.General.Internal("password");

        var user = User.Create(
            mail.Value,
            passwordHash,
            fullname.Value,
            request.Nickname,
            birthDay,
            request.Description);

        if (user.IsFailure)
            return user.Error;

        await _writeDbContext.Users.AddAsync(user.Value, ct);
        await _writeDbContext.SaveChangesAsync(ct);
        return Result.Success();
    }
}