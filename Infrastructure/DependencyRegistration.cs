using Infrastructure.Commands.UserCreate;
using Infrastructure.DbContexts;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<SocialWriteDbContext>();
        services.AddScoped<CreateUserCommand>();
        return services;
    }
}