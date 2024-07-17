using Infrastructure.Commands.AddFriend;
using Infrastructure.Commands.UserCreate;
using Infrastructure.DbContexts;
using Infrastructure.Queries.GetUserById;
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

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<GetUserByIdQuery>();
        return services;
    }
}