namespace Application.DTO;

public class UserDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } 
    public string SecondName { get; init; } 
    public string Nickname { get; init; } 
    public DateOnly? BirthDate { get; init; } 
    public string? Description { get; init; } 
    public PhotoDto? Avatar { get; set; } 

    public UserDto()
    {
        
    }

    public UserDto(Guid id, string firstName, string secondName, string nickname, DateOnly? birthDate, string? description, PhotoDto? avatar)
    {
        Id = id;
        FirstName = firstName;
        SecondName = secondName;
        Nickname = nickname;
        BirthDate = birthDate;
        Description = description;
        Avatar = avatar;
    }
    
}