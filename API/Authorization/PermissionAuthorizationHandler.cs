using API.Attributes;
using Domain.Constants;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;

namespace API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<HasPermissionAttribute>
{
    private readonly ILogger<PermissionAuthorizationHandler> _logger;
    public PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionAttribute requirement)
    {
        var permissions = context.User.Claims
            .Where(c => c.Type == AuthenticationConstants.Permission)
            .Select(p => p.Value);
        if (!permissions.Contains(requirement.Permission))
        {
            _logger.LogError("User has not permission: {permission}", requirement.Permission);
            return Task.CompletedTask;
        }
        
        _logger.LogInformation("Access authorised");
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}