using Domain.Common;
using Domain.Entities;

namespace Application.Providers;

public interface IJwtProvider
{
    Result<string> Generate(User user);
}