using Microsoft.Extensions.DependencyInjection;
namespace Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}