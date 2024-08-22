using Application.DataAccess;
using Application.Providers;
using Domain.Common;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Login;

public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITransaction _transaction;
    private readonly IConfiguration _configuration;
    private readonly IJwtProvider _jwtProvider;

    public LoginHandler(ITransaction transaction,
        IConfiguration configuration,
        IUserRepository userRepository,
        IJwtProvider jwtProvider)
    {
        _transaction = transaction;
        _configuration = configuration;
        _jwtProvider = jwtProvider;
        _userRepository = userRepository;
    }

    public async Task<Result<LoginResponse>> Handle(HttpContext context, LoginRequest request, CancellationToken ct)
    {
        var userResult = await _userRepository.GetByEmail(request.Email, ct);
        if (userResult.IsFailure)
            return userResult.Error;
        var user = userResult.Value;

        var isVerify = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isVerify)
            return Errors.UserErrors.InvalidCredentials();

        var accessToken = _jwtProvider.GenerateAccessToken(user).Value;
        var refreshToken = _jwtProvider.GenerateRefreshToken().Value;
        //TODO вынести
        var refresh = RefreshToken.Create(refreshToken,
            DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:ExpiresRefresh"] ?? string.Empty))).Value;

        user.UpdateRefresh(refresh);
        await _transaction.SaveChangesAsync(ct);

        context.Response.Cookies.Append("yummy-cookies", accessToken,
            new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddSeconds(24)),
                HttpOnly = true,
                Secure = true,
            });

        return new LoginResponse(accessToken, refreshToken, user.Role.Name);
    }
}