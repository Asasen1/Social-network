using Infrastructure.Commands.AddFriend;
using Infrastructure.Commands.DeletePhoto;
using Infrastructure.Commands.UploadPhoto;
using Infrastructure.Commands.UserCreate;
using Infrastructure.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : ApplicationController
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateUserCommand command,
        [FromForm] CreateUserRequest request,
        CancellationToken ct)
    {
        var idResult = await command.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetById(
        [FromServices] GetUserByIdQuery query,
        [FromQuery]GetUserByIdRequest request,
        CancellationToken ct)
    {
        var idResult = await query.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok(idResult.Value);
    }

    [HttpPost("friend")]
    public async Task<IActionResult> PublishFriend(
        [FromServices] AddFriendCommand command,
        AddFriendRequest request,
        CancellationToken ct)
    {
        var idResult = await command.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok();
    }

    [HttpPost("photo")]
    public async Task<IActionResult> PublishPhoto(
        [FromServices] UploadPhotoCommand command,
        [FromForm] UploadPhotoRequest request,
        CancellationToken ct)
    {
        var idResult = await command.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok();
    }
    [HttpDelete("photo")]
    public async Task<IActionResult> DeletePhoto(
        [FromServices] DeletePhotoCommand command,
        [FromForm] DeletePhotoRequest request,
        CancellationToken ct)
    {
        var idResult = await command.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok();
    }
}