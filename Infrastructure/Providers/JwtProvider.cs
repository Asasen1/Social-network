using System.Reactive.Subjects;
using System.Security.Claims;
using System.Text;
using Application.Providers;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
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
    public Result<string> Generate(User user)
    {
        var jwtHandler = new JsonWebTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var permissionClaims = user.Role.Permissons
            .Select(p => new Claim(AuthenticationConstants.Permissions, p));
        var claims = permissionClaims.Concat([
            new Claim(AuthenticationConstants.UserId, user.Id.ToString()),
            new Claim(AuthenticationConstants.Role, user.Role.Name)
        ]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(claims),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256),
            Expires = DateTime.UtcNow.AddHours(_options.Expires)
        };
        var token = jwtHandler.CreateToken(tokenDescriptor);
        return token;
    }
}