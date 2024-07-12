using Infrastructure.Commands.AddFriend;
using Infrastructure.Commands.UserCreate;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateUserCommand command,
        CreateUserRequest request,
        CancellationToken ct)
    {
        var idResult = await command.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok(idResult.Value);
    }

    [HttpPost("Friend")]
    public async Task<IActionResult> PublishFriend(
        [FromServices] AddFriendCommand command,
        [FromQuery] AddFriendRequest request,
        CancellationToken ct)
    {
        var idResult = await command.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok(idResult.Value);
    }
}