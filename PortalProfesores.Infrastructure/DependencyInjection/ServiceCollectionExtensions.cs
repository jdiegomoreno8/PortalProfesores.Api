using Microsoft.Extensions.DependencyInjection;
using PortalProfesores.Domain.Interfaces.Repositories;
using PortalProfesores.Domain.Interfaces.Services;
using PortalProfesores.Infrastructure.Repositories;
using PortalProfesores.Infrastructure.Services;
using PortalProfesores.Infrastructure.Services.Catalogos;


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

        services.AddScoped<IProfesorDetalleRepository, ProfesorDetalleRepository>();    
				
		services.AddScoped<IPersonaRepository, PersonaRepository>();

        services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();

        services.AddScoped<ICiudadRepository, CiudadRepository>();




        #endregion

        #region Servicios

        services.AddScoped<IHealthService, HealthService>();

        services.AddScoped<IUsuarioService, UsuarioService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IPasswordService, PasswordService>();

        services.AddScoped<IProfesorService, ProfesorService>();

        services.AddScoped<IAdministradorService, AdministradorService>();

        services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();

        services.AddScoped<ICiudadRepository, CiudadRepository>();

        services.AddScoped<ICatalogoService, CatalogoService>();

        #endregion

        return services;
    }
}