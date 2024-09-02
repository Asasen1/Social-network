using Application.DTO;
using Application.Providers;
using Domain.Common;

namespace Application.Features.RefreshToken;

public class RefreshTokenHandler
{
    private readonly IFileProvider _provider;

    public RefreshTokenHandler(IFileProvider provider)
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