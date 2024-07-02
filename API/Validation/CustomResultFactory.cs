﻿using API.Contracts;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace API.Validation;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails)
    {
        if (validationProblemDetails is null)
        {
            throw new("ValidationProblemDetails is null");
        }

        List<ErrorInfo> errorInfos = [];
        foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
        {
            var errors = validationErrors
                .Select(Error.Deserialize)
                .Select(e => new ErrorInfo(e, invalidField));

            errorInfos.AddRange(errors);
        }

        var envelope = Envelope.Error(errorInfos.ToArray());

        return new BadRequestObjectResult(envelope);
    }
}