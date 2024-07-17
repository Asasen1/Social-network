using Application.Abstractions;
using Application.DTO;
using Domain.Common;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries.GetUserById;

public class GetUserByIdQuery(ReadDbContext dbContext) :
    IQueryHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdRequest request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct);
        if (user == null)
            return Errors.General.NotFound(request.Id);
        var userDto = new UserDto(
            user.FirstName,
            user.SecondName,
            user.Nickname,
            user.BirthDate,
            user.Description);
        return new GetUserByIdResponse(userDto);
    }
}