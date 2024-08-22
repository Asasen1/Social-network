namespace Application.Features.Login;

public record LoginResponse(string AccessToken, string RefershToken, string Role);