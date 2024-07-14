using Infrastructure.Commands.AddFriend;
using Infrastructure.Commands.UserCreate;
using Infrastructure.DbContexts;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<ReadDbContext>();
        services.AddCommands();
        services.AddQueries();
        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<CreateUserCommand>();
        services.AddScoped<AddFriendCommand>();
        return services;
    }

    static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services;
    }
}