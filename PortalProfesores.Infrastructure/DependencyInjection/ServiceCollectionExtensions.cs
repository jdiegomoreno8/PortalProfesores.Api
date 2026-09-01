using Microsoft.Extensions.DependencyInjection;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Repositories;
using PortalProfesores.Infrastructure.Services;

namespace PortalProfesores.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Repositorios
        services.AddScoped<IHealthRepository, HealthRepository>();

        // Servicios
        services.AddScoped<IHealthService, HealthService>();

        return services;
    }
}