﻿using Application.Features;
using Domain.AgregateRoot;
using Domain.Common;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly WriteDbContext _dbContext;

    public UserRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<User>> GetByEmail(string email, CancellationToken ct)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken: ct);
        if (user is null)
            return Errors.General.NotFound();
        return user;
    }
    public async Task<Result<User>> GetById(Guid id, CancellationToken ct)
    {
        var user = await _dbContext.Users.FindAsync(new object?[] { id }, cancellationToken: ct);
        if (user is null)
            return Errors.General.NotFound();
        return user;
    }
}