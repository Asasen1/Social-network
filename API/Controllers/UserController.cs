using API.Attributes;
using Application.Features.Login;
using Domain.Common;
using Infrastructure.Commands.AddFriend;
using Infrastructure.Commands.DeletePhoto;
using Infrastructure.Commands.UploadPhoto;
using Infrastructure.Commands.UserCreate;
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
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromServices] LoginHandler handler,
        [FromForm] LoginRequest request,
        CancellationToken ct)
    {
        var result = await handler.Handle(HttpContext, request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HasPermission(Permissions.Post.Delete)]
    [HttpGet]
    public async Task<IActionResult> GetById(
        [FromServices] GetUserByIdQuery query,
        [FromQuery]GetUserByIdRequest request,
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
}