using API.Requests;
using Application.Providers;
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
    private const string KEY = "user"; 
    private readonly ICacheProvider _cacheProvider;
    public UserController(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }
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
        using var stream = request.File.OpenReadStream();
        var data = new UploadPhotoData(
            request.UserId, 
            stream, 
            request.File.FileName, 
            request.File.ContentType, 
            request.File.Length, 
            request.IsMain);
        
        var result = await command.Handle(data, ct);
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
    [HttpGet("all")]
    public async Task<IActionResult> GetUsers(
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