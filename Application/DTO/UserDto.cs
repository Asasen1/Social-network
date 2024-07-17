using Domain.Entities.Photos;

namespace Application.DTO;

public record UserDto(string FirstName,
    string SecondName,
    string Nickname,
    DateOnly? BirthDate,
    string? Description);