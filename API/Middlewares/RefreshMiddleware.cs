using Application.Providers;

namespace API.Middlewares;

public class RefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RefreshMiddleware> _logger;

    public RefreshMiddleware(RequestDelegate next, ILogger<RefreshMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Cookies.ContainsKey("yummy-cookies"))
        {
            var jwtProvider =
                context.RequestServices.GetRequiredService<IJwtProvider>();
            await jwtProvider.Refresh(context,
                context.Request.Cookies["yummy-cookies"]!);
            _logger.LogInformation("Tokens are update");
        }

        await _next(context);
    }
}