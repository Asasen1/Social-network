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
        if (!idResult.IsSuccess)
            return BadRequest(idResult.Error);
        return Ok(idResult.Value);
    }

  
    
}