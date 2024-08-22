using Domain.Agregates;
using Domain.Common;
using Domain.Entities;

namespace Application.Providers;

public interface IJwtProvider
{
    Result<string> GenerateAccessToken(User user);
    public Result<string> GenerateRefreshToken();
}