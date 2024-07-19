namespace Application.DTO;

public record UserDto(
    Guid Id,
    string FirstName,
    string SecondName,
    string Nickname,
    DateOnly? BirthDate,
    string? Description,
    PhotoDto? Avatar = null!);