using System.Runtime.InteropServices.JavaScript;
using Application.DTO;
using Application.Providers;
using Domain.Common;

namespace Application.Features.RefreshToken;

public class RefreshTokenHandler
{
    private readonly IJwtProvider _provider;

    public RefreshTokenHandler(IJwtProvider provider)
    {
        _provider = provider;
    }

    public async Task<Result<TokenDto>> Handle(TokenDto tokenDto, CancellationToken ct)
    {
        var principal = _provider.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        if (principal.IsFailure)
            return principal.Error;

        var user = await _provider.CheckExpired(principal.Value, tokenDto.RefreshToken, ct);
        if (user.IsFailure)
            return user.Error;

        return await _provider.UpdateTokens(user.Value, ct);
    }
} 