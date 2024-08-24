using System.Net;
using System.Security.Authentication;
using API.Contracts;
using Domain.Common;

namespace API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex.Source);
            _logger.LogError(ex.StackTrace);

            if (ex.Message.StartsWith("IDX10223"))
            {
                var errorInfoU = new ErrorInfo(Errors.General.TokenSmell
                    (ex.Message.Replace("IDX10223", "")));
                var envelopeU = Envelope.Error([errorInfoU]);
            
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(envelopeU);
                return;
            }
            var errorInfo = new ErrorInfo(Errors.General.Iternal(ex.Message));
            var envelope = Envelope.Error([errorInfo]);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}