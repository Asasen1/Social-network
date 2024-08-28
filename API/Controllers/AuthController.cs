using API.Handlers;
using Application.DTO;
using Application.Features.Login;
using Application.Features.RefreshToken;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController : ApplicationController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromServices] LoginHandler handler,
        [FromForm] LoginRequest request,
        CancellationToken ct)
    {
        var result = await handler.Handle(HttpContext, request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
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

        cookieHandler.Handle(HttpContext, "yummy-cookies", result.Value.AccessToken, 15);

        return Ok(result.Value);
    }
}