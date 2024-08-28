using Infrastructure.Commands.AddFriend;
using Infrastructure.Commands.DeletePhoto;
using Infrastructure.Commands.UploadPhoto;
using Infrastructure.Commands.UserCreate;
using Infrastructure.Queries.GetAllUsers;
using Infrastructure.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateUserCommand command,
        [FromForm] CreateUserRequest request,
        CancellationToken ct)
    {
        var result = await command.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok();
    }


    // [HasPermission(Permissions.Post.Delete)]
    [HttpGet]
    public async Task<IActionResult> GetById(
        [FromServices] GetUserByIdQuery query,
        [FromQuery] GetUserByIdRequest request,
        CancellationToken ct)
    {
        var result = await query.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPost("friend")]
    public async Task<IActionResult> PublishFriend(
        [FromServices] AddFriendCommand command,
        AddFriendRequest request,
        CancellationToken ct)
    {
        var result = await command.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok();
    }

    [HttpPost("photo")]
    public async Task<IActionResult> PublishPhoto(
        [FromServices] UploadPhotoCommand command,
        [FromForm] UploadPhotoRequest request,
        CancellationToken ct)
    {
        var result = await command.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok();
    }

    [HttpDelete("photo")]
    public async Task<IActionResult> DeletePhoto(
        [FromServices] DeletePhotoCommand command,
        [FromForm] DeletePhotoRequest request,
        CancellationToken ct)
    {
        var result = await command.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok();
    }
    [HttpGet("test")]
    public async Task<IActionResult> Test(
        [FromServices] GetAllUsersQuery query,
        [FromQuery] GetAllUsersRequest request,
        CancellationToken ct)
    {
        var result = await query.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }
}