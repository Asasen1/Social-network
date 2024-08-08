using Application.Providers;
using Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Login;

public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
        _userRepository = userRepository;
    }
    public async Task<Result<LoginResponse>> Handle(HttpContext context, LoginRequest request, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmail(request.Email, ct);
        if (user.IsFailure)
            return user.Error;
        
        var isVerify = BCrypt.Net.BCrypt.Verify(request.Password, user.Value.PasswordHash);
        if (!isVerify)
            return Errors.UserErrors.InvalidCredentials();
        
        var token = _jwtProvider.Generate(user.Value).Value;
        context.Response.Cookies.Append("yummy-cookies", token);
        
        return new LoginResponse(token, user.Value.Role.Name);
    }
}