using Application.DTO;
using Domain.Agregates;
using Domain.Common;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Providers;

public interface IJwtProvider
{
    Result<string> GenerateAccessToken(User user);
    public Result<RefreshToken> GenerateRefreshToken();
    public Task<Result<TokenDto>> Refresh(HttpContext context, TokenDto tokenDto, CancellationToken ct = default);
}