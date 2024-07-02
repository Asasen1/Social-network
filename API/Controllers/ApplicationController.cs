﻿using API.Contracts;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase
{
    protected new ActionResult Ok(object? result = null)
    {
        var envelope = Envelope.Ok(result);

        return base.Ok(envelope);
    }

    protected IActionResult BadRequest(Error? error)
    {
        var envelope = Envelope.Ok(error);

        return base.BadRequest(envelope);
    }

    protected IActionResult NotFound(Error? error)
    {
        var envelope = Envelope.Ok(error);

        return base.NotFound(envelope);
    }
}