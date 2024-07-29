using Application.Providers;
using Infrastructure.Commands.AddFriend;
using Infrastructure.Commands.UploadPhoto;
using Infrastructure.Commands.UserCreate;
using Infrastructure.DbContexts;
using Infrastructure.Options;
using Infrastructure.Providers;
using Infrastructure.Queries.GetUserById;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;

namespace Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<ReadDbContext>();
        services.AddProviders();
        services.AddCommands();
        services.AddQueries();
        services.AddDataStorages(configuration);
        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<UploadPhotoCommand>();
        services.AddScoped<CreateUserCommand>();
        services.AddScoped<AddFriendCommand>();
        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<GetUserByIdQuery>();
        return services;
    }
    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IMinIoProvider, MinIoProvider>();
        return services;
    }
    private static IServiceCollection AddDataStorages(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.Minio)
                .Get<MinioOptions>() ?? throw new("Minio configuration not found");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(false);
        });
        return services;
    }
}