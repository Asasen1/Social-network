using Infrastructure.Commands.AddFriend;
using Infrastructure.Commands.UserCreate;
using Infrastructure.Queries.GetUserById;
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
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetById(
        [FromServices] GetUserByIdQuery query,
        [FromQuery] GetUserByIdRequest request,
        CancellationToken ct)
    {
        var idResult = await query.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok(idResult.Value);
    }

    [HttpPost("Friend")]
    public async Task<IActionResult> PublishFriend(
        [FromServices] AddFriendCommand command,
        [FromBody] AddFriendRequest request,
        CancellationToken ct)
    {
        var idResult = await command.Handle(request, ct);
        if (idResult.IsFailure)
            return BadRequest(idResult.Error);
        return Ok();
    }
}