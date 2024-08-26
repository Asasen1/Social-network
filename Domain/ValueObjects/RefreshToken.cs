using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public class RefreshToken : ValueObject
{
    public string Token { get; }
    public DateTime Expires { get; }
    
    
    public RefreshToken(string token, DateTime expires)
    {
        Token = token;
        Expires = expires;
    }

    public static Result<RefreshToken> Create(string token, DateTime expires)
    {
        return new RefreshToken(token, expires);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Token;
        yield return Expires;
    }
}