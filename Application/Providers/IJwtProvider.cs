﻿using System.Security.Claims;
using Application.DTO;
using Domain.AgregateRoot;
using Domain.Common;
using Domain.ValueObjects;

namespace Application.Providers;

public interface IJwtProvider
{
    Result<string> GenerateAccessToken(User user);
    public Result<RefreshToken> GenerateRefreshToken();
    public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string accessToken);
    public Task<Result<User>> CheckExpired(ClaimsPrincipal principal, string refreshToken, CancellationToken ct);
    public Task<Result<TokenDto>> UpdateTokens(User user, CancellationToken ct);
}