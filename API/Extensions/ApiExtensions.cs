using System.Net;
using System.Security.Authentication;
using System.Text;
using API.Authorization;
using API.Contracts;
using Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace API.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    LifetimeValidator = (before, expires, token, parameters) =>
                        expires != null && expires > DateTime.UtcNow,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                configuration.GetSection("Jwt:SecretKey").Get<string>()
                                ?? throw new AuthenticationException()))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["yummy-cookies"];
                        return Task.CompletedTask;
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        var errorInfo = new ErrorInfo(Errors.General.UnAuthorized("User is unauthorized"));
                        var envelope = Envelope.Error([errorInfo]);

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsJsonAsync(envelope);
                    },
                    OnForbidden = async context =>
                    {
                        var errorInfo = new ErrorInfo(Errors.General.Forbidden("User has not permission."));
                        var envelope = Envelope.Error([errorInfo]);

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        await context.Response.WriteAsJsonAsync(envelope);
                    }
                };
            });
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new()
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureLogging(this IServiceCollection services,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(configuration.GetSection("Seq").Value
                         ?? throw new ApplicationException("Seq configuration is empty"))
            .CreateLogger();
        services.AddSerilog();
        return services;
    }
}