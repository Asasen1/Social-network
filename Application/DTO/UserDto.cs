namespace Application.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } 
    public string SecondName { get; set; } 
    public string Nickname { get; set; } 
    public DateOnly? BirthDate { get; set; } 
    public string? Description { get; set; }

    public UserDto()
    {
        
    }
    public UserDto(Guid id, string firstName, string secondName, string nickname, DateOnly? birthDate, string? description)
    {
        Id = id;
        FirstName = firstName;
        SecondName = secondName;
        Nickname = nickname;
        BirthDate = birthDate;
        Description = description;
    }
    
}