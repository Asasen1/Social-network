﻿using Application.Features.Login;
using Application.Features.RefreshToken;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyRegistration).Assembly);
        services.AddScoped<LoginHandler>();
        services.AddScoped<RefreshTokenHandler>();
        return services;
    }
}