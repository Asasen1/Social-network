﻿using Application.Features;
using Application.Providers;
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
        services.AddScoped<WriteDbContext>();
        services.AddScoped<ReadDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddProviders();
        services.AddCommandsAndQueries();
        services.AddDataStorages(configuration);
        services.ConfigureOptions(configuration);
        return services;
    }

    private static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblies(typeof(DependencyRegistration).Assembly)
            .AddClasses(filter => filter.Where(x => x.Name.EndsWith("Command") ||
                                                    x.Name.EndsWith("Query")))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip));
        return services;
    }
    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IMinioProvider, MinioProvider>();
        services.AddScoped<IJwtProvider, JwtProvider>();
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
    private static IServiceCollection ConfigureOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
        return services;
    }
}