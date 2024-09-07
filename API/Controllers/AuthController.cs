using API.Handlers;
using Application.DTO;
using Application.Features.Login;
using Application.Features.RefreshToken;
using Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers;

public class AuthController : ApplicationController
{
    private readonly JwtOptions _jwtOptions;

    public AuthController(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromServices] LoginHandler handler,
        [FromServices] CookieHandler cookieHandler,
        [FromForm] LoginRequest request,
        CancellationToken ct)
    {
        var result = await handler.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        cookieHandler.Handle(
            HttpContext,
            "yummy-cookies",
            result.Value.AccessToken,
            _jwtOptions.ExpiresAccess);

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens([FromForm] TokenDto dto,
        [FromServices] RefreshTokenHandler tokenHandler,
        [FromServices] CookieHandler cookieHandler,
        CancellationToken ct)
    {
        var result = await tokenHandler.Handle(dto, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        cookieHandler.Handle(
            HttpContext,
            "yummy-cookies",
            result.Value.AccessToken,
            _jwtOptions.ExpiresAccess);

        return Ok(result.Value);
    }
}