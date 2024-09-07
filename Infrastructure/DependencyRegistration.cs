using Application.DataAccess;
using Application.Features;
using Application.Providers;
using Domain.Common;
using Infrastructure.DbContexts;
using Infrastructure.Options;
using Infrastructure.Providers;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Scrutor;

namespace Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProviders();
        services.AddCommandsAndQueries();
        services.AddDataStorages(configuration);
        services.ConfigureOptions(configuration);
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblies(typeof(DependencyRegistration).Assembly)
            .AddClasses(filter => filter.Where(x => x.Name.EndsWith("Command") ||
                                                    x.Name.EndsWith("Query")))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsSelfWithInterfaces().WithScopedLifetime());
        return services;
    }
    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IFileProvider, FileProvider>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ITransaction, Transaction>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
    private static IServiceCollection AddDataStorages(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<ReadDbContext>();
        
        services.AddSingleton<SqlConnectionFactory>();
        
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
    private static IServiceCollection ConfigureOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
        return services;
    }
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    { 
        services.AddTransient<IUserRepository, UserRepository>();
        return services;
    }
}