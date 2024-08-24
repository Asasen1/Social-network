﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DataAccess;
using Application.DTO;
using Application.Features;
using Application.Providers;
using Domain.Agregates;
using Domain.Common;
using Domain.Constants;
using Domain.ValueObjects;
using Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Providers;

public class JwtProvider : IJwtProvider
{
    private readonly IUserRepository _repository;
    private readonly ITransaction _transaction;
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options,
        IUserRepository repository,
        ITransaction transaction)
    {
        _repository = repository;
        _transaction = transaction;
        _options = options.Value;
    }

    public Result<string> GenerateAccessToken(User user)
    {
        var jwtHandler = new JsonWebTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var permissionClaims = user.Role.Permissions
            .Select(p => new Claim(AuthenticationConstants.Permission, p));
        var claims = permissionClaims.Concat([
            new Claim(AuthenticationConstants.UserId, user.Id.ToString()),
            new Claim(AuthenticationConstants.Role, user.Role.Name)
        ]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(claims),
            SigningCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256),
            Expires = DateTime.UtcNow.AddSeconds(_options.ExpiresAccess)
        };
        var token = jwtHandler.CreateToken(tokenDescriptor);
        return token;
    }

    public Result<RefreshToken> GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];
        using var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(randomNumbers);
        return RefreshToken.Create(Convert.ToBase64String(randomNumbers),
            DateTime.UtcNow.AddDays(_options.ExpiresRefresh));
    }


    public async Task<Result<TokenDto>> Refresh(HttpContext context,
        string accessToken,
        CancellationToken ct = default)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal.IsFailure)
            return principal.Error;

        var user = await CheckExpired(principal.Value, ct);
        if (user.IsFailure)
            return user.Error;

        return await UpdateTokens(context, user.Value, ct);
    }

    private Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string accessToken)
    {
        if (accessToken.IsEmpty())
            return Errors.General.ValueIsRequired();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException();
        return claimsPrincipal;
    }

    private async Task<Result<User>> CheckExpired(
        ClaimsPrincipal principal, CancellationToken ct)
    {
        var userIds = principal.Claims.Where(c => c.Type == AuthenticationConstants.UserId)
            .Select(t => t.Value);
        var userId = userIds.SingleOrDefault();
        var isGuid = Guid.TryParse(userId, out var id);
        if (!isGuid)
            return Errors.General.Iternal("Error with refresh");

        var userResult = await _repository.GetById(id, ct);
        if (userResult.IsFailure)
            return userResult.Error;
        var user = userResult.Value;

        if (user.RefreshToken.Expires <= DateTime.UtcNow)
            return Errors.General.TokenSmell("Expiration time is failure or invalid token");
        return user;
    }

    private async Task<Result<TokenDto>> UpdateTokens(
        HttpContext context,
        User user,
        CancellationToken ct)
    {
        var accessToken = GenerateAccessToken(user).Value;
        var refreshToken = GenerateRefreshToken().Value;
        user.UpdateRefresh(refreshToken);
        context.Response.Cookies.Append("yummy-cookies", accessToken,
            new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddHours(_options.ExpiresAccess)),
                HttpOnly = true,
                Secure = true,
            });
        await _transaction.SaveChangesAsync(ct);
        return new TokenDto(accessToken, refreshToken.Token);
    }
}