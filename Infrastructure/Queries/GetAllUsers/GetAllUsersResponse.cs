using Application.DTO;

namespace Infrastructure.Queries.GetAllUsers;

public record GetAllUsersResponse(List<UserDto> Users);