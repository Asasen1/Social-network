namespace Infrastructure.Commands.UserCreate;

public record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string SecondName,
    string Nickname,
    string? BirthDate,
    string? Description);