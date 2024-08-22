namespace Infrastructure.Options;

public class JwtOptions
{
    public const string Jwt = nameof(Jwt);
    public string SecretKey { get; init; } = string.Empty;
    public int ExpiresAccess { get; init; }
    public int ExpiresRefresh { get; init; }
}