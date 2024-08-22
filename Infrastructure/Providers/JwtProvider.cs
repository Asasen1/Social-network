using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Providers;
using Domain.Agregates;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.DbContexts;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Providers;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    public JwtProvider(IOptions<JwtOptions> options)
    {
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

    public Result<string> GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];
        using var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(randomNumbers);
        return Convert.ToBase64String(randomNumbers);
    }

    public Result GetPrincipalFromExpiredToken()
    {
        return Result.Success();
    }
}