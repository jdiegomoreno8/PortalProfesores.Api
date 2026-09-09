using Microsoft.Extensions.DependencyInjection;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Repositories;
using PortalProfesores.Infrastructure.Services;


namespace PortalProfesores.Infrastructure.DependencyInjection;

/// <summary>
/// Centraliza el registro de las dependencias utilizadas por la aplicación.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        #region Repositorios

        services.AddScoped<IHealthRepository, HealthRepository>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
		
		services.AddScoped<IUsuarioService, UsuarioService>();
		
		services.AddScoped<IAuthService, AuthService>();
		
		services.AddScoped<IJwtService, JwtService>();
		
		services.AddScoped<IPasswordService, PasswordService>();
		
		services.AddScoped<IJwtService, JwtService>();
		
		services.AddScoped<IPersonaRepository, PersonaRepository>();
		
		services.AddScoped<IProfesorService, ProfesorService>();


        #endregion

        #region Servicios

        services.AddScoped<IHealthService, HealthService>();

        #endregion

        return services;
    }
}