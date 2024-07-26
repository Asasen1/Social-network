namespace Infrastructure.Commands.UserCreate;

public record CreateUserRequest(
    string FirstName,
    string SecondName,
    string Nickname,
    string BirthDate,
    string? Description);