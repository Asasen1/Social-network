namespace Infrastructure.Commands.UserCreate;

public record CreateUserRequest(
    string FirstName,
    string SecondName,
    string Email,
    string Nickname,
    string BirthDate,
    string? Description);