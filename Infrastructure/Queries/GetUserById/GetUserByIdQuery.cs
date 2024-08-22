using Application.Abstractions;
using Application.DTO;
using Domain.Common;
using Infrastructure.DbContexts;
using Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries.GetUserById;

public class GetUserByIdQuery(ReadDbContext dbContext) :
    IQueryHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdRequest request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.Id == request.Id, ct);
        if (user == null)
            return Errors.General.NotFound(request.Id);
        var avatar = user.Photos.FirstOrDefault(p => p.IsMain == true) ?? new UserPhotoReadModel();
        var userDto = new UserDto(
            user.Id,
            user.FirstName,
            user.SecondName,
            user.Nickname,
            user.BirthDate,
            user.Description,
            new PhotoDto(avatar.Id, avatar.Path, avatar.IsMain));
        return new GetUserByIdResponse(userDto);
    }
}