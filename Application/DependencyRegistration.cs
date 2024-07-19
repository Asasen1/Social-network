using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyRegistration).Assembly);
        return services;
    }
}