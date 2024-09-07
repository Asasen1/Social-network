using Domain.Common;

namespace API.Handlers;

public class CookieHandler
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public CookieHandler(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }
    public void Handle(HttpContext context, string key, string value, int expires)
    {
        context.Response.Cookies.Append(key, value,
            new CookieOptions
            {
                Expires = new DateTimeOffset(_dateTimeProvider.AddHours(expires)),
                HttpOnly = true,
                Secure = true,
            });
    }
}