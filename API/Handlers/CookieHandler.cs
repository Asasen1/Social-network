using Application.DataAccess;

namespace API.Handlers;

public class CookieHandler
{
    public void Handle(HttpContext context, string key, string value, int expires)
    {
        context.Response.Cookies.Append(key, value,
            new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddHours(expires)),
                HttpOnly = true,
                Secure = true,
            });
    }
}