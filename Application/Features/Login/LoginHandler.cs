using Application.DataAccess;
using Application.Providers;
using Domain.Common;

namespace Application.Features.Login;

public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITransaction _transaction;
    private readonly IJwtProvider _jwtProvider;

    public LoginHandler(ITransaction transaction,
        IUserRepository userRepository,
        IJwtProvider jwtProvider)
    {
        _transaction = transaction;
        _jwtProvider = jwtProvider;
        _userRepository = userRepository;
    }

    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken ct)
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

        user.UpdateRefresh(refreshToken);
        await _transaction.SaveChangesAsync(ct);

        return new LoginResponse(accessToken, refreshToken.Token, user.Role.Name);
    }
}